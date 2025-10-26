using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Context;

namespace MCan.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 5;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMore(int skip = 0)
        {
            var clients = await _context.Clients
                .OrderBy(c => c.Id)
                .Skip(skip)
                .Take(PageSize)
                .Select(c => new { c.Id, c.Name, c.Image, c.Link })
                .ToListAsync();

            return Json(new { items = clients, count = clients.Count });
        }
    }
}
