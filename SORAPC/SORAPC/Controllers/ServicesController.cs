using Microsoft.AspNetCore.Mvc;

namespace SORAPC.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Services()
        {
            return View();
        }
    }
}