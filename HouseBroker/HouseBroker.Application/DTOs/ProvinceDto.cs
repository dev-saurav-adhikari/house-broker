using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Application.DTOs;

public class ProvinceDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class UpsertProvinceDto
{
    [Required]
    [StringLength(100)]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;
}
