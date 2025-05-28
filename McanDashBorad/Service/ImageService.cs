public interface IImageService
{
    Task<string> UploadImageAsync(IFormFile imageFile, string folderName);
    void DeleteImage(string relativePath, string folderName);
}
