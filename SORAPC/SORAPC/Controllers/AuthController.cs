using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SORAPC_API.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SORAPC.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApiService _apiService;

        public AuthController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.Register(model);
                if (result.Contains("успешно"))
                    return RedirectToAction("Login");
                ViewBag.ErrorMessage = result; // Отображаем сообщение об ошибке
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _apiService.Login(model);
                if (response != null)
                {
                    // Сохраняем токен в сессии
                    HttpContext.Session.SetString("Token", response.Token);

                    // Извлекаем ID пользователя из токена или API (добавим позже)
                    // Для простоты пока предположим, что у вас есть метод в ApiService для получения профиля
                    var user = await _apiService.GetUserProfileByLogin(model.Login, response.Token);
                    if (user != null)
                    {
                        HttpContext.Session.SetInt32("UserId", user.IdUsers ?? 0);

                        // Устанавливаем аутентификацию через куки
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Login),
                            new Claim(ClaimTypes.Role, user.RoleId == 1 ? "Администратор" : "Клиент")
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        // Перенаправляем на страницу профиля
                        return RedirectToAction("Profile");
                    }
                    ModelState.AddModelError("", "Не удалось загрузить данные пользователя.");
                }
                ViewBag.ErrorMessage = "Неверный логин или пароль";
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var token = HttpContext.Session.GetString("Token");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (string.IsNullOrEmpty(token) || !userId.HasValue)
            {
                return RedirectToAction("Login");
            }

            var user = await _apiService.GetUserProfile(userId.Value, token);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("Token");
            HttpContext.Session.Remove("UserId");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}