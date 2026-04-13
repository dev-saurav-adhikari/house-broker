using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HouseBroker.Application.Attribute;
using Microsoft.AspNetCore.Http;
using HouseBroker.Domain.Enums;

namespace HouseBroker.Application.DTOs;

public class InsertPropertyDetailDto : PropertyDetailDto
{
    [AllowedExtension([".jpg", ".jpeg", ".png"], ErrorMessage = "Please upload a valid image file.")]
    public required IFormFile ImageFile { get; set; }
}

public class PropertyDetailWithBrokerInfoDto : PropertyDetailDto
{
    public long Id { get; set; }
    public long BrokerId { get; set; }
    public string BrokerName { get; set; } = string.Empty;
    public string BrokerEmail { get; set; } = string.Empty;
    public string BrokerPhone { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    public decimal EstimatedCommission { get; set; }
}

public class PropertyFilterDto
{
    public string? Search { get; set; }
    
    [Range(0, long.MaxValue)]
    public long? ProvinceId { get; set; }
    
    [Range(0, long.MaxValue)]
    public long? DistrictId { get; set; }
    
    [Range(0, int.MaxValue)]
    public int? WardNumber { get; set; }
    public PropertyTypeEnum? PropertyType { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal? MinPrice { get; set; }
    
    [Range(0, double.MaxValue)]
    public decimal? MaxPrice { get; set; }

    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}

public class PropertyDetailDto
{
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        public required string Description { get; set; }
    
        [Range(0, long.MaxValue)]
        public long ProvinceId { get; set; }
    
        [Range(0, long.MaxValue)]
        public long DistrictId { get; set; }
    
        [StringLength(250)]
        public string Municipality { get; set; } = string.Empty;
    
        [Range(0, int.MaxValue)]
        public int WardNumber { get; set; }
    
        [StringLength(500)]
        public string LandMark { get; set; } = string.Empty;
    
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public PropertyTypeEnum PropertyType { get; set; } 
    
}

public class UpdatePropertyDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    [AllowedExtension([".jpg", ".jpeg", ".png"], ErrorMessage = "Please upload a valid image file.")]
    public IFormFile? ImageFile { get; set; }
    public long? ProvinceId { get; set; }
    public long? DistrictId { get; set; }
    public string? Municipality { get; set; }
    public int? WardNumber { get; set; }
    public string? LandMark { get; set; }
    public decimal? Price { get; set; }
    public PropertyTypeEnum? PropertyType { get; set; }
    public bool? IsAvailable { get; set; }
}