using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Interfaces.IServices;

public interface IPropertyService
{
    Task<List<PropertyDto>> GetAllProperties();
    Task InsertProperty(InsertPropertyDto property, long userId);
}