namespace CovaldysPilot.Application.Interfaces.Services;

public interface IAuthService
{
  Task<AuthResultDto> RegisterAsync(RegisterDto dto);
  Task<AuthResultDto> LoginAsync(LoginDto dto);
  Task<AuthResultDto> RefreshTokenAsync(string refreshToken);
  Task RevokeTokenAsync(string refreshToken);
}