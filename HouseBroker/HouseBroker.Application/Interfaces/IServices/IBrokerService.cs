using HouseBroker.Application.Common;
using HouseBroker.Application.DTOs;
using HouseBroker.Infrastructure.Services;

namespace HouseBroker.Application.Interfaces.IServices;

public interface IBrokerService
{
    Task<Pagination<BrokerDto>> GetBrokersAsync(BasicFilterDto filter);
    Task<BrokerDto?> GetBrokerByIdAsync(long id);
    Task<BrokerPropertiesDto> GetPropertiesByBrokerIdAsync(long brokerId, BasicFilterDto filter);
}
