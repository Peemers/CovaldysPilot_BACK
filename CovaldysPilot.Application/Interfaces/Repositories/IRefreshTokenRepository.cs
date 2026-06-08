using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
  Task<RefreshToken?> GetByTokenAsync(string token);
  Task RevokeTokenAsync(string token);
}