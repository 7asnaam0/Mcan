using Models.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;

public class ProjectService
{
    private readonly ApplicationDbContext _context;
    private readonly IImageService _imageService;

    public ProjectService(ApplicationDbContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }

    public async Task<List<Project>> GetAllAsync() => await _context.Projects.ToListAsync();

    public async Task<Project?> GetByIdAsync(int id) => await _context.Projects.FindAsync(id);

    public async Task AddAsync(Project project, IFormFile? imageFile)
    {
        if (imageFile != null)
        {
            project.ImageUrl = await _imageService.UploadImageAsync(imageFile, "projects");
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Project project, IFormFile? imageFile)
    {
        var existing = await _context.Projects.FindAsync(project.Id);
        if (existing == null) return;

        existing.TitleAr = project.TitleAr;
        existing.TitleEn = project.TitleEn;
        existing.DescriptionAr = project.DescriptionAr;
        existing.DescriptionEn = project.DescriptionEn;
        existing.ProjectLink = project.ProjectLink;

        if (imageFile != null)
        {
            _imageService.DeleteImage(existing.ImageUrl ?? "", "projects");
            existing.ImageUrl = await _imageService.UploadImageAsync(imageFile, "projects");
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Projects.FindAsync(id);
        if (existing == null) return;

        _imageService.DeleteImage(existing.ImageUrl ?? "", "projects");
        _context.Projects.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
