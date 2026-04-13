using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseBroker.Domain.Entities;

public class District : BaseEntity
{
    [Required]
    [StringLength(100)]
    [MinLength(1)]
    public string Name { get; set; } 
    
    [Required]
    public long ProvinceId { get; set; }

    [ForeignKey(nameof(ProvinceId))]
    public Province Province { get; set; } = null!;
}