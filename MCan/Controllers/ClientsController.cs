using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Models.Context;

namespace MCan.Controllers
{
    public class ClientsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        private const int PageSize = 5;

        public ClientsViewComponent(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            // جلب أول 5 عملاء للعرض الأولي
            var clients = await _context.Clients
                .OrderBy(c => c.Id)
                .Take(PageSize)
                .ToListAsync();

            var total = await _context.Clients.CountAsync();

            ViewBag.TotalClients = total;
            ViewBag.Loaded = clients.Count;
            ViewBag.PageSize = PageSize;

            return View(clients);
        }
    
        [HttpGet]
        public async Task<IViewComponentResult> GetMore(int skip = 0)
        {
            var clients = await _context.Clients
                .OrderBy(c => c.Id)
                .Skip(skip)
                .Take(PageSize)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Image,
                    c.Link
                })
                .ToListAsync();

            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                items = clients,
                count = clients.Count
            });

            return new ContentViewComponentResult(json);
        }

    }

}