namespace HouseBroker.Application.DTOs;

public class LoginRequestDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class LoginResponseDto
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    
}