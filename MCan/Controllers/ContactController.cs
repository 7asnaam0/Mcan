using Microsoft.AspNetCore.Mvc;

using Models;
using Models.Context;

namespace MCan.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContactMessage model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                _context.SaveChanges();
                return Json(new { success = true, message = "Message sent successfully!" });
            }

            var errors = ModelState.Where(x => x.Value.Errors.Any())
                                   .ToDictionary(
                                       kvp => kvp.Key,
                                       kvp => kvp.Value.Errors.First().ErrorMessage
                                   );

            return Json(new { success = false, errors });
        }

    }
}
