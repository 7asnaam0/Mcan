using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Context;

namespace AdminDashborad.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context) { 
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _context.ContactMessages.ToListAsync();
            return View(projects);
        }
    }
}
