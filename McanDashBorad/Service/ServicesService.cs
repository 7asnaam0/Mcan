using Models.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;

public class ServiceService
{
    private readonly ApplicationDbContext _context;
    private readonly IImageService _imageService;

    public ServiceService(ApplicationDbContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }

    public async Task<List<Service>> GetAllAsync() => await _context.Services.ToListAsync();

    public async Task<Service?> GetByIdAsync(int id) => await _context.Services.FindAsync(id);

    public async Task AddAsync(Service service, IFormFile? imageFile)
    {
        if (imageFile != null)
        {
            service.ImageUrl = await _imageService.UploadImageAsync(imageFile, "services");
        }

        _context.Services.Add(service);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Service service, IFormFile? imageFile)
    {
        var existing = await _context.Services.FindAsync(service.Id);
        if (existing == null) return;

        existing.TitleAr = service.TitleAr;
        existing.TitleEn = service.TitleEn;
        existing.DescriptionAr = service.DescriptionAr;
        existing.DescriptionEn = service.DescriptionEn;

        if (imageFile != null)
        {
            _imageService.DeleteImage(existing.ImageUrl ?? "", "services");
            existing.ImageUrl = await _imageService.UploadImageAsync(imageFile, "services");
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Services.FindAsync(id);
        if (existing == null) return;

        _imageService.DeleteImage(existing.ImageUrl ?? "", "services");
        _context.Services.Remove(existing);
        await _context.SaveChangesAsync();
    }
}
