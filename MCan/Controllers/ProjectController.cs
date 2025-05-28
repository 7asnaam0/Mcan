using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Context;

public class ProjectViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public ProjectViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var projects = await _context.Projects.ToListAsync();
        return View(projects); 
    }
}
