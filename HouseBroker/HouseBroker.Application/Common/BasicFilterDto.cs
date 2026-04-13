using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Application.Common;

public class BasicFilterDto
{
    public string? Search { get; set; }
    
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;
    
    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}