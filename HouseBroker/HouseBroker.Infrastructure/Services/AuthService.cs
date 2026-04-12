using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Interfaces.IServices;
using HouseBroker.Application.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HouseBroker.Infrastructure.Services;

public class AuthService(UserManager<IdentityUser<long>> _userManager, 
                        SignInManager<IdentityUser<long>> _signInManager, 
                        IOptions<JwtSettings> _jwtSettings)
    : IAuthService
{
    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.Username);
        if (user != null)
        {
            var signinResult = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
            if (signinResult.Succeeded)
            {
                return new LoginResponseDto
                {
                    AccessToken = await GenerateAccessToken(user),
                    RefreshToken = await GenerateRefreshToken(user)
                };
            }
        }
        throw new UnauthorizedAccessException("Invalid username or password");
    }
    

    private async Task<string> GenerateAccessToken(IdentityUser<long> user, bool isPasswordReset = false)
    { 
        var jwtSettings = _jwtSettings.Value;
        var userRoles = await _userManager.GetRolesAsync(user);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email!),
            new Claim("email",user.Email!),
            new Claim("Id", user.Id.ToString()),
            new Claim("role", string.Join(",", userRoles) ),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
        );

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<string> GenerateRefreshToken(IdentityUser<long> user)
    {
        var jwtSettings = _jwtSettings.Value;
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
        );
        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: [new Claim("id", user.Id.ToString()), new Claim("email", user.Email!),],
            expires: DateTime.UtcNow.AddMinutes(jwtSettings.RefreshTokenExpirationDays),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}