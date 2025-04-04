using Microsoft.AspNetCore.Mvc;

namespace SORAPC.Controllers
{
    public class CompanyController : Controller
    {
        public IActionResult Company()
        {
            return View();
        }
    }
}