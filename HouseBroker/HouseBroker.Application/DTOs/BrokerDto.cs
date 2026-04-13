using System.ComponentModel.DataAnnotations;
using HouseBroker.Infrastructure.Services;

namespace HouseBroker.Application.DTOs;

public class BrokerDto
{
    public long Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}

public class BrokerPropertiesDto
{
    public long Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public Pagination<BrokerPropertyDto>? Properties { get; set; } = null;
}

public class BrokerPropertyDto : PropertyDto
{
    public long Id { get; set; }
    public bool IsAvailable { get; set; }
    public decimal EstimatedCommission  { get; set; }
}