using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HouseBroker.Domain.Enums;

namespace HouseBroker.Domain.Entities;

public class Property : BaseEntity
{
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    public required string Description { get; set; }
    [StringLength(250)]
    public string ImageUrl { get; set; }
    [ForeignKey(nameof(Province))]
    public long ProvinceId { get; set; }
    [ForeignKey(nameof(District))]
    public long DistrictId { get; set; }
    [StringLength(250)]
    public string Municipality { get; set; } = string.Empty;
    public int WardNumber { get; set; }
    [StringLength(500)]
    public string LandMark { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public PropertyTypeEnum PropertyType { get; set; } 
    public long BrokerId { get; set; }
    
    public bool IsAvailable { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal EstimatedCommission { get; set; }

    public virtual Province Province { get; set; }
    public virtual District District { get; set; }
    
}