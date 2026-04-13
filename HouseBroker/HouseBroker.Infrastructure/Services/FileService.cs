using HouseBroker.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HouseBroker.Infrastructure.Services;

public class FileService(   IHttpContextAccessor _httpContextAccessor) : IFileService
{
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0) return string.Empty;

        var folderName = "Uploads";
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
    
        // Generate unique name to avoid overwriting
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var fullPath = Path.Combine(pathToSave, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return the relative URL path
        return $"/content/{fileName}";
    }

    public string GetFileUrl(string filePath)
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        var baseUrl = $"{request?.Scheme}://{request!.Host}";
        return $"{baseUrl}{filePath}";

    }

    public void RemoveFile(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl)) return;

        var fileName = fileUrl.Replace("/content/", "");
        if (fileName.StartsWith("/"))
        {
            fileName = fileName.Substring(1);
        }

        var folderName = "Uploads";
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderName, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}