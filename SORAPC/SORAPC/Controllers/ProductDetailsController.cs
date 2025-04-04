using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using SORAPC_API.Models;

namespace SORAPC.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly SorapcContext db;

        public ProductDetailsController(SorapcContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("ProductDetails/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p => p.Reviews)
                .ThenInclude(r => r.Users)
                .FirstOrDefaultAsync(p => p.IdProduct == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [Route("ProductDetails/AddReview")]
        public async Task<IActionResult> AddReview(Review review)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Находим пользователя в базе данных по имени (User.Identity.Name)
                var user = await db.Users
                    .FirstOrDefaultAsync(u => u.Logins == User.Identity.Name);

                if (user == null)
                {
                    // Если пользователь не найден, возвращаем ошибку
                    return BadRequest("Пользователь не найден.");
                }

                // Присваиваем UsersId из найденного пользователя
                review.UsersId = user.IdUsers;
                review.CreatedAt = DateTime.Now;

                db.Reviews.Add(review);
                await db.SaveChangesAsync();
            }
            else
            {
                // Если пользователь не авторизован, перенаправляем на страницу логина
                return RedirectToAction("Login", "Auth");
            }

            return RedirectToAction("Details", new { id = review.ProductId });
        }
    }
}