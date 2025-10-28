using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCan.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {

            ViewBag.BaseUrl = _configuration["AppSettings:BaseUrl"];
            return View();
;
        }
    }
}
