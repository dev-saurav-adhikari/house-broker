using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using HouseBroker.Domain.Enums;

namespace HouseBroker.Application.DTOs;

public class InsertPropertyDto
{
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    public required string Description { get; set; }
    public required IFormFile ImageFile { get; set; }
    public long ProvinceId { get; set; }
    public long DistrictId { get; set; }
    [StringLength(250)]
    public string Municipality { get; set; } = string.Empty;
    public int WardNumber { get; set; }
    [StringLength(500)]
    public string LandMark { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public PropertyTypeEnum PropertyType { get; set; } 
}

public class PropertyDto 
{
    public long Id { get; set; }
    public long BrokerId { get; set; }
    public string BrokerName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    public required string Description { get; set; }
    public long ProvinceId { get; set; }
    public long DistrictId { get; set; }
    [StringLength(250)]
    public string Municipality { get; set; } = string.Empty;
    public int WardNumber { get; set; }
    [StringLength(500)]
    public string LandMark { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public PropertyTypeEnum PropertyType { get; set; }
    public decimal EstimatedCommission { get; set; }
}