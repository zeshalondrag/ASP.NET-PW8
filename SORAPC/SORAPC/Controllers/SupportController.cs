using Microsoft.AspNetCore.Mvc;

namespace SORAPC.Controllers
{
    public class SupportController : Controller
    {
        public IActionResult Support()
        {
            return View();
        }
    }
}