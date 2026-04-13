using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseBroker.Application.DTOs;

public class CommissionDto
{
    public long Id { get; set; }
    public decimal MinimumAmount { get; set; }
    public double MaximumAmount { get; set; }
    public decimal Rate { get; set; }
}

public class UpsertCommissionDto
{
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, double.MaxValue)]
    public decimal MinimumAmount { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, double.MaxValue)]
    public double MaximumAmount { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, 100, ErrorMessage = "Rate must be between 0 and 100")]
    public decimal Rate { get; set; }
}
