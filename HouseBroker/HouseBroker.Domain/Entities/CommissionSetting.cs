using System.ComponentModel.DataAnnotations.Schema;
namespace HouseBroker.Domain.Entities;

public class CommissionSetting : BaseEntity
{
    [Column(TypeName = "decimal(18, 2)")]
    public decimal MinimumAmount { get; set; } = 0;
    [Column(TypeName = "decimal(18, 2)")]
    public double MaximumAmount { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Rate { get; set; }
}