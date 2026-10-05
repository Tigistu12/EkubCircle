using EkubCircle.Application.DTOs.Auth;

namespace EkubCircle.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterOrganizerAsync(RegisterRequestDto request);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
}