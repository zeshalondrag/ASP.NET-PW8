using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SORAPC_API.Models;
using System.Threading.Tasks;

namespace SORAPC.Controllers
{
    [Authorize(Roles = "Администратор")] 
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Панель администратора
        public IActionResult Admin()
        {
            return View(); 
        }

        // Статусы заказов
            // Таблица статусов заказа
            public async Task<IActionResult> OrderStatus()
            {
                var token = HttpContext.Session.GetString("Token");
                var orderStatuses = await _apiService.GetOrderStatuses(token); 
                return View(orderStatuses);
            }

            // Детали статуса заказа
            [HttpGet]
            public async Task<IActionResult> DetailsOrderStatus(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var orderStatus = await _apiService.GetOrderStatus(id, token);
                return View("~/Views/Admin/OrderStatus/DetailsOrderStatus.cshtml", orderStatus);
            }

            // Создание статуса заказа
            [HttpGet]
            public IActionResult CreateOrderStatus()
            {
                return View("~/Views/Admin/OrderStatus/CreateOrderStatus.cshtml");
            }

            [HttpPost]
            public async Task<IActionResult> CreateOrderStatus(OrderStatus orderStatus)
            {
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("Token");
                    await _apiService.CreateOrderStatus(orderStatus, token);
                    return RedirectToAction("OrderStatus");
                }
                return View(orderStatus);
            }

            // Редактирование статуса заказа
            [HttpGet]
            public async Task<IActionResult> EditOrderStatus(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var orderStatus = await _apiService.GetOrderStatus(id, token);
                return View("~/Views/Admin/OrderStatus/EditOrderStatus.cshtml", orderStatus);
            }

            [HttpPost]
            public async Task<IActionResult> EditOrderStatus(int id, OrderStatus orderStatus)
            {
                if (id != orderStatus.IdOrderStatus)
                {
                    return BadRequest();
                }
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("Token");
                    await _apiService.UpdateOrderStatus(id, orderStatus, token);
                    return RedirectToAction("OrderStatus");
                }
                return View(orderStatus);
            }

            // Удаление статуса заказа
            [HttpGet]
            public async Task<IActionResult> DeleteOrderStatus(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var orderStatus = await _apiService.GetOrderStatus(id, token);
                return View("~/Views/Admin/OrderStatus/DeleteOrderStatus.cshtml", orderStatus);
            }

            [HttpPost, ActionName("DeleteOrderStatus")]
            public async Task<IActionResult> DeleteOrderStatusConfirmed(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                await _apiService.DeleteOrderStatus(id, token);
                return RedirectToAction("OrderStatus");
            }

        // Категории продуктов
        // Таблица категорий продуктов
        public async Task<IActionResult> ProductCategory()
            {
                var token = HttpContext.Session.GetString("Token");
                var productCategories = await _apiService.GetProductCategories(token); 
                return View(productCategories);
            }

            // Детали категории продукта
            [HttpGet]
            public async Task<IActionResult> DetailsProductCategory(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var productCategory = await _apiService.GetProductCategory(id, token);
                return View("~/Views/Admin/ProductCategory/DetailsProductCategory.cshtml", productCategory);
            }

            // Создание категории продукта
            [HttpGet]
            public IActionResult CreateProductCategory()
            {
                return View("~/Views/Admin/ProductCategory/CreateProductCategory.cshtml");
            }

            [HttpPost]
            public async Task<IActionResult> CreateProductCategory(ProductCategory productCategory)
            {
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("Token");
                    await _apiService.CreateProductCategory(productCategory, token);
                    return RedirectToAction("ProductCategory");
                }
                return View(productCategory);
            }

            // Редактирование категории продукта
            [HttpGet]
            public async Task<IActionResult> EditProductCategory(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var productCategory = await _apiService.GetProductCategory(id, token);
                return View("~/Views/Admin/ProductCategory/EditProductCategory.cshtml", productCategory);
            }

            [HttpPost]
            public async Task<IActionResult> EditProductCategory(int id, ProductCategory productCategory)
            {
                if (id != productCategory.IdProductCategory)
                {
                    return BadRequest();
                }
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("Token");
                    await _apiService.UpdateProductCategory(id, productCategory, token);
                    return RedirectToAction("ProductCategory");
                }
                return View(productCategory);
            }

            // Удаление категории продукта
            [HttpGet]
            public async Task<IActionResult> DeleteProductCategory(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var productCategory = await _apiService.GetProductCategory(id, token);
                return View("~/Views/Admin/ProductCategory/DeleteProductCategory.cshtml", productCategory);
            }

            [HttpPost, ActionName("DeleteProductCategory")]
            public async Task<IActionResult> DeleteProductCategoryConfirmed(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                await _apiService.DeleteProductCategory(id, token);
                return RedirectToAction("ProductCategory");
            }

        // Продукты
        // Таблица продуктов
        public async Task<IActionResult> Products()
            {
                var token = HttpContext.Session.GetString("Token");
                var products = await _apiService.GetProducts(token);
                return View(products);
            }

            // Детали продукта
            [HttpGet]
            public async Task<IActionResult> DetailsProducts(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var product = await _apiService.GetProduct(id, token);
                if (product == null)
                {
                    return NotFound();
                }
                return View("~/Views/Admin/Products/DetailsProducts.cshtml", product);
            }

            // Создание продукта
            [HttpGet]
            public IActionResult CreateProducts()
            {
                return View("~/Views/Admin/Products/CreateProducts.cshtml");
            }

            [HttpPost]
            public async Task<IActionResult> CreateProducts(Product product)
            {
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("Token");
                    await _apiService.CreateProduct(product, token);
                    return RedirectToAction("Products");
                }
                return View(product);
            }

            // Редактирование продукта
            [HttpGet]
            public async Task<IActionResult> EditProducts(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var product = await _apiService.GetProduct(id, token);
                return View("~/Views/Admin/Products/EditProducts.cshtml", product);
            }

            [HttpPost]
            public async Task<IActionResult> EditProducts(int id, Product product)
            {
                if (id != product.IdProduct)
                {
                    return BadRequest();
                }
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("Token");
                    await _apiService.UpdateProduct(id, product, token);
                    return RedirectToAction("Products");
                }
                return View(product);
            }

            // Удаление продукта
            [HttpGet]
            public async Task<IActionResult> DeleteProducts(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var product = await _apiService.GetProduct(id, token);
                if (product == null)    
                {
                    return NotFound();
                }
                return View("~/Views/Admin/Products/DeleteProducts.cshtml", product);
            }

            [HttpPost, ActionName("DeleteProducts")]
            public async Task<IActionResult> DeleteProductConfirmed(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                await _apiService.DeleteProduct(id, token);
                return RedirectToAction("Products");
            }

        // Статус продукта
        // Таблица статусов продукта
        public async Task<IActionResult> ProductStatus()
            {
                var token = HttpContext.Session.GetString("Token");
                var productStatuses = await _apiService.GetProductStatuses(token); 
                return View(productStatuses);
            }

            // Детали статуса продукта
            [HttpGet]
            public async Task<IActionResult> DetailsProductStatus(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var productStatus = await _apiService.GetProductStatus(id, token);
                return View("~/Views/Admin/ProductStatus/DetailsProductStatus.cshtml", productStatus);
            }

            // Создание статуса продукта
            [HttpGet]
            public IActionResult CreateProductStatus()
            {
                return View("~/Views/Admin/ProductStatus/CreateProductStatus.cshtml");
            }

            [HttpPost]
            public async Task<IActionResult> CreateProductStatus(ProductStatus productStatus)
            {
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("Token");
                    await _apiService.CreateProductStatus(productStatus, token);
                    return RedirectToAction("ProductStatus");
                }
                return View(productStatus);
            }

            // Редактирование статуса продукта
            [HttpGet]
            public async Task<IActionResult> EditProductStatus(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var productStatus = await _apiService.GetProductStatus(id, token);
                return View("~/Views/Admin/ProductStatus/EditProductStatus.cshtml", productStatus);
            }

            [HttpPost]
            public async Task<IActionResult> EditProductStatus(int id, ProductStatus productStatus)
            {
                if (id != productStatus.IdProductStatus)
                {
                    return BadRequest();
                }
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("Token");
                    await _apiService.UpdateProductStatus(id, productStatus, token);
                    return RedirectToAction("ProductStatus");
                }
                return View(productStatus);
            }

            // Удаление статуса продукта
            [HttpGet]
            public async Task<IActionResult> DeleteProductStatus(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var productStatus = await _apiService.GetProductStatus(id, token);
                return View("~/Views/Admin/ProductStatus/DeleteProductStatus.cshtml", productStatus);
            }

            [HttpPost, ActionName("DeleteProductStatus")]
            public async Task<IActionResult> DeleteProductStatusConfirmed(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                await _apiService.DeleteProductStatus(id, token);
                return RedirectToAction("ProductStatus");
            }

        // Роли
            // Таблица ролей
            public async Task<IActionResult> Roles()
            {
                var token = HttpContext.Session.GetString("Token");
                var roles = await _apiService.GetRoles(token);
                return View(roles);
            }
            // Детали роли
            [HttpGet]
            public async Task<IActionResult> DetailsRoles(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var role = await _apiService.GetRole(id, token); 
                return View("~/Views/Admin/Roles/DetailsRoles.cshtml", role);
            }

            // Создание роли
            [HttpGet]
            public IActionResult CreateRoles()
            {
                return View("~/Views/Admin/Roles/CreateRoles.cshtml");
            }

            [HttpPost]
            public async Task<IActionResult> CreateRoles(Role role)
            {
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("Token");
                    await _apiService.CreateRole(role, token); 
                    return RedirectToAction("Roles");
                }
                return View(role);
            }

            // Редактирование роли
            [HttpGet]
            public async Task<IActionResult> EditRoles(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var role = await _apiService.GetRole(id, token); // 
                return View("~/Views/Admin/Roles/EditRoles.cshtml", role);
            }

            [HttpPost]
            public async Task<IActionResult> EditRoles(int id, Role role)
            {
                if (id != role.IdRole)
                {
                    return BadRequest();
                }
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Session.GetString("Token");
                    await _apiService.UpdateRole(id, role, token); 
                    return RedirectToAction("Roles");
                }
                return View(role);
            }

            // Удаление роли
            [HttpGet]
            public async Task<IActionResult> DeleteRoles(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var role = await _apiService.GetRole(id, token); 
                if (role == null)
                {
                    return NotFound();
                }
                return View("~/Views/Admin/Roles/DeleteRoles.cshtml", role);
            }

            [HttpPost, ActionName("DeleteRoles")]
            public async Task<IActionResult> DeleteRolesConfirmed(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                await _apiService.DeleteRole(id, token); 
                return RedirectToAction("Roles");
            }

        // Пользователи
            // Таблица пользователей
            public async Task<IActionResult> Users()
            {
                var token = HttpContext.Session.GetString("Token");
                var users = await _apiService.GetUsers(token);
                return View(users);
            }
            // Детали пользователя
            [HttpGet]
            public async Task<IActionResult> DetailsUsers(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var user = await _apiService.GetUser(id, token);
                return View("~/Views/Admin/Users/DetailsUsers.cshtml", user);
            }

            // Создание пользователя
            [HttpGet]
            public IActionResult CreateUsers()
            {
                return View("~/Views/Admin/Users/CreateUsers.cshtml");
            }

            [HttpPost]
            public async Task<IActionResult> CreateUsers(User user)
            {
                var token = HttpContext.Session.GetString("Token");
                await _apiService.CreateUser(user, token);
                return RedirectToAction("Users");
            }

            // Редактирования пользователя
            [HttpGet]
            public async Task<IActionResult> EditUsers(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var user = await _apiService.GetUser(id, token);
                return View("~/Views/Admin/Users/EditUsers.cshtml", user);
            }

            [HttpPost]
            public async Task<IActionResult> EditUsers(int id, User user)
            {
                var token = HttpContext.Session.GetString("Token");
                await _apiService.UpdateUser(id, user, token);
                return RedirectToAction("Users");
            }

            // Удаление пользователя
            [HttpGet]
            public async Task<IActionResult> DeleteUsers(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                var user = await _apiService.GetUser(id, token);
                return View("~/Views/Admin/Users/DeleteUsers.cshtml", user);
            }

            [HttpPost, ActionName("DeleteUsers")]
            public async Task<IActionResult> DeleteUsersConfirmeds(int id)
            {
                var token = HttpContext.Session.GetString("Token");
                await _apiService.DeleteUser(id, token);
                return RedirectToAction("Users");
            }
    }
}