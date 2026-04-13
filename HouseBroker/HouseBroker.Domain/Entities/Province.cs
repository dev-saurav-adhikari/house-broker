using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Domain.Entities;

public class Province : BaseEntity
{
    [Required]
    [StringLength(100)]
    [MinLength(1)]
    public string Name { get; set; } 
    public ICollection<District> Districts { get; set; } = new List<District>();
    
}