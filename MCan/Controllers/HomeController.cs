using Microsoft.AspNetCore.Mvc;

namespace MCan.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
