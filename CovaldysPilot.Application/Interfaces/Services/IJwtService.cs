using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IJwtService
{
  string GenerateAccessToken(User user);
  string GenerateRefreshToken();
  DateTime GetRefreshTokenExpiryDate();
}