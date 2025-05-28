using Microsoft.AspNetCore.Mvc;
using Models.Context;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace McanDashBorad.ViewComponents
{
    public class ServicesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ServicesViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var services = await _context.Services.ToListAsync();
            Console.WriteLine("Services Count: " + services.Count);
            return View(services);
        }
    }
}
