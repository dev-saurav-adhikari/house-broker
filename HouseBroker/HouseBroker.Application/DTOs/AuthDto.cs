using System.ComponentModel.DataAnnotations;

namespace HouseBroker.Application.DTOs;

public class LoginRequestDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginResponseDto
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    
}

public class RegisterRequestDto
{
    public required string Username { get; set; }
    [StringLength(12, MinimumLength = 8)]
    public required string Password { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    [Phone]
    public required string PhoneNumber { get; set; }
    
}