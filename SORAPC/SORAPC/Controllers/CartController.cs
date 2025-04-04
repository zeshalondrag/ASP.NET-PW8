using Microsoft.AspNetCore.Mvc;
using SORAPC.Models;
using SORAPC_API.Models;
using System.Threading.Tasks;

namespace SORAPC.Controllers
{
    public class CartController : Controller
    {
        private readonly ApiService _apiService;

        public CartController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Отображение корзины
        public async Task<IActionResult> Cart()
        {
            // Получаем токен из сессии или куки
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Получаем корзину пользователя через API
            var cartItems = await _apiService.GetUserCart(token);
            return View(cartItems);
        }

        // Добавление товара в корзину
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1, string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Получаем токен
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Получаем пользователя
            var user = await _apiService.GetUserProfileByLogin(User.Identity.Name, token);
            if (user == null)
            {
                return BadRequest("Пользователь не найден.");
            }

            // Получаем продукт
            var product = await _apiService.GetProduct(productId, token);
            if (product == null)
            {
                return NotFound("Товар не найден.");
            }

            // Создаём элемент корзины
            var cartItem = new Cart
            {
                UsersId = user.IdUsers,
                ProductId = productId,
                Quantity = quantity,
                Price = product.Price
            };

            // Добавляем в корзину через API
            var result = await _apiService.AddToCart(cartItem, token);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Не удалось добавить товар в корзину.");
            }

            // Перенаправляем обратно
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Cart");
        }

        // Удаление товара из корзины
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Получаем токен
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Удаляем элемент корзины через API
            var result = await _apiService.RemoveFromCart(cartId, token);
            return RedirectToAction("Cart");
        }
    }
}