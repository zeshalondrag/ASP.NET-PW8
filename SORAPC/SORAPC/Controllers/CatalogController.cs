using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC.Models;
using SORAPC_API.Models;

namespace SORAPC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ApiService _apiService;

        public CatalogController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Catalog()
        {
            var products = await _apiService.GetProducts();
            return View(products);
        }
    }
}