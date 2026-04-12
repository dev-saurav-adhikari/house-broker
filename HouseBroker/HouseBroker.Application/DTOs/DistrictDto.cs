using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Application.DTOs;

public class DistrictDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long ProvinceId { get; set; }
    public string ProvinceName { get; set; } = string.Empty;
}

public class UpsertDistrictDto
{
    [Required]
    [StringLength(100)]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public long ProvinceId { get; set; }
}
