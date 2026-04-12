using Microsoft.AspNetCore.Http;

namespace HouseBroker.Application.Interfaces.IServices;

public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file);
}