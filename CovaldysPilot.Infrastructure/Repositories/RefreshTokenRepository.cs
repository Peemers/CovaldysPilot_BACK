using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace CovaldysPilot.Infrastructure.Repositories;

public class RefreshTokenRepository(CovaldysPilotDbContext context) : IRefreshTokenRepository
{
  public async Task<RefreshToken?> GetByIdAsync(Guid id)
    => await context.RefreshTokens.FindAsync(id);

  public async Task<IEnumerable<RefreshToken>> GetAllAsync()
    => await context.RefreshTokens.ToListAsync();

  //on charge l'utilisateur associé en même temps car on en aura besoin pour générer le nouveau jwt.
  public async Task<RefreshToken?> GetByTokenAsync(string token)
    => await context.RefreshTokens
      .Include(rt => rt.User)
      .FirstOrDefaultAsync(rt => rt.Token == token);

  public async Task RevokeTokenAsync(string token)
  {
    var refreshToken = await GetByTokenAsync(token);
    if (refreshToken != null)
    {
      refreshToken.RevokedAt = DateTime.UtcNow;
      context.RefreshTokens.Update(refreshToken);
    }
  }

  public async Task AddAsync(RefreshToken refreshToken)
    => await context.RefreshTokens.AddAsync(refreshToken);

  public Task UpdateAsync(RefreshToken refreshToken)
  {
    context.RefreshTokens.Update(refreshToken);
    return Task.CompletedTask;
  }

  public async Task DeleteAsync(Guid id)
  {
    var refreshToken = await GetByIdAsync(id);
    if (refreshToken != null)
      context.RefreshTokens.Remove(refreshToken);
  }

  public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();
}