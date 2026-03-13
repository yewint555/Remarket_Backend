using Microsoft.AspNetCore.Hosting;

namespace Application.Services;

public interface IImageService
{
    Task<string> SaveImageAsync(string? base64String, string folderName);
}

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _env;

    public ImageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveImageAsync(string? base64String, string folderName)
    {
        if (string.IsNullOrWhiteSpace(base64String))
            return null!;

        try
        {
            var base64Data = base64String.Contains(",") 
                ? base64String.Split(',')[1] 
                : base64String;

            byte[] imageBytes = Convert.FromBase64String(base64Data);

            string wwwRootPath = _env.WebRootPath;
            if (string.IsNullOrEmpty(wwwRootPath))
            {
                wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            if (!Directory.Exists(wwwRootPath))
                Directory.CreateDirectory(wwwRootPath);

            var relativeFolder = Path.Combine("uploads", folderName);
            var absoluteFolder = Path.Combine(wwwRootPath, relativeFolder);

            if (!Directory.Exists(absoluteFolder))
                Directory.CreateDirectory(absoluteFolder);

            var fileName = $"{Guid.NewGuid()}.png";
            var filePath = Path.Combine(absoluteFolder, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes);
            return $"/{relativeFolder}/{fileName}".Replace("\\", "/");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Image Save Error: {ex.Message}");
            return null!;
        }
    }
}