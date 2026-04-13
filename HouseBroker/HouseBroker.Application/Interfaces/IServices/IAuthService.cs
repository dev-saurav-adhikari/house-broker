using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Interfaces.IServices;

public interface IAuthService
{
    Task<LoginResponseDto> Login(LoginRequestDto loginRequset);
    Task RegisterHouseSeeker(RegisterRequestDto registerRequset);
    Task RegisterHouseBroker(RegisterRequestDto registerRequset);
}