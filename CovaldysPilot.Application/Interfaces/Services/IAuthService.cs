using CovaldysPilot.Application.DTOs.Auth.Request;
using CovaldysPilot.Application.DTOs.Auth.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IAuthService
{
  Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
  Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
  Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);
  Task RevokeTokenAsync(string refreshToken);
  Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto dto);
}