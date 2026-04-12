using HouseBroker.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HouseBroker.Infrastructure.Services;

public class FileService : IFileService
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
}