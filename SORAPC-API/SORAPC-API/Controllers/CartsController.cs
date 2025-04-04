using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SORAPC_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly SorapcContext _context;

        public CartsController(SorapcContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        // Получение всех элементов корзины (только для администраторов)
        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            return await _context.Carts
                .Include(c => c.Product)
                .Include(c => c.Users)
                .ToListAsync();
        }

        // GET: api/Carts/user
        // Получение корзины текущего пользователя
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Cart>>> GetUserCart()
        {
            // Получаем ID пользователя из токена
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Не удалось определить пользователя.");
            }

            var cartItems = await _context.Carts
                .Include(c => c.Product)
                .Where(c => c.UsersId == userId)
                .ToListAsync();

            return Ok(cartItems);
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Администратор")]
        public async Task<ActionResult<Cart>> GetCart(int? id)
        {
            var cart = await _context.Carts
                .Include(c => c.Product)
                .Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.IdCart == id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // POST: api/Carts
        // Добавление товара в корзину
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Cart>> AddToCart(Cart cart)
        {
            // Проверяем, авторизован ли пользователь
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Не удалось определить пользователя.");
            }

            // Проверяем, что пользователь добавляет товар в свою корзину
            if (cart.UsersId != userId)
            {
                return Unauthorized("Вы можете добавлять товары только в свою корзину.");
            }

            // Проверяем, существует ли продукт
            var product = await _context.Products.FindAsync(cart.ProductId);
            if (product == null)
            {
                return NotFound("Товар не найден.");
            }

            // Проверяем, есть ли уже этот товар в корзине
            var existingCartItem = await _context.Carts
                .FirstOrDefaultAsync(c => c.UsersId == userId && c.ProductId == cart.ProductId);

            if (existingCartItem != null)
            {
                // Если товар уже есть, увеличиваем количество
                existingCartItem.Quantity += cart.Quantity;
            }
            else
            {
                // Иначе добавляем новый элемент
                cart.Price = product.Price;
                _context.Carts.Add(cart);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.IdCart }, cart);
        }

        // DELETE: api/Carts/5
        // Удаление товара из корзины
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCart(int? id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound("Элемент корзины не найден.");
            }

            // Проверяем, принадлежит ли элемент корзины текущему пользователю
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Не удалось определить пользователя.");
            }

            if (cart.UsersId != userId)
            {
                return Unauthorized("Вы можете удалять только свои элементы корзины.");
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Carts/5
        // Обновление элемента корзины (например, количества)
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCart(int? id, Cart cart)
        {
            if (id != cart.IdCart)
            {
                return BadRequest();
            }

            // Проверяем, принадлежит ли элемент корзины текущему пользователю
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Не удалось определить пользователя.");
            }

            if (cart.UsersId != userId)
            {
                return Unauthorized("Вы можете редактировать только свои элементы корзины.");
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool CartExists(int? id)
        {
            return _context.Carts.Any(e => e.IdCart == id);
        }
    }
}