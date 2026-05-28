using CovaldysPilot.Application.DTOs.SignIn.Request;
using CovaldysPilot.Application.DTOs.SignIn.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface ISignInService
{
  Task<SignInResponseDto> RegisterAsync(Guid userId, CreateSignInRequestDto dto);
  Task UnregisterAsync(Guid userId, Guid signInId);
  Task<IEnumerable<SignInResponseDto>> GetByEventAsync(Guid eventId);
  Task<IEnumerable<SignInResponseDto>> GetByUserAsync(Guid userId);
}