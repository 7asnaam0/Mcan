using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class ImageService : IImageService
{
    private readonly string _wwwRootPath;

    public ImageService(IWebHostEnvironment env)
    {
        _wwwRootPath = env.WebRootPath;
    }

    public async Task<string> UploadImageAsync(IFormFile imageFile, string folderName)
    {
        if (imageFile == null || imageFile.Length == 0)
            return "";

        string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        string extension = Path.GetExtension(imageFile.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            throw new Exception("نوع الملف غير مدعوم");

        string uniqueFileName = Guid.NewGuid().ToString() + extension;
        string folderPath = Path.Combine(_wwwRootPath, "images", folderName);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        // Return relative path to be saved in the database
        return Path.Combine("images", folderName, uniqueFileName).Replace("\\", "/");
    }

    public void DeleteImage(string relativePath, string folderName)
    {
        if (string.IsNullOrEmpty(relativePath))
            return;

        string fullPath = Path.Combine(_wwwRootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
