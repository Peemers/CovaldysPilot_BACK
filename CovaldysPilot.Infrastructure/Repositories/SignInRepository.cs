using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace CovaldysPilot.Infrastructure.Repositories;

public class SignInRepository(CovaldysPilotDbContext context) : ISignInRepository
{
  public async Task<SignIn?> GetByIdAsync(Guid id)
    => await context.SignIns.FindAsync(id);

  public async Task<IEnumerable<SignIn>> GetAllAsync()
    => await context.SignIns.ToListAsync();

  public async Task<SignIn?> GetByUserAndEventAsync(Guid userId, Guid eventId)
    => await context.SignIns
      .FirstOrDefaultAsync(s => s.UserId == userId && s.EventId == eventId);

  public async Task<IEnumerable<SignIn>> GetByEventAsync(Guid eventId)
    => await context.SignIns
      .Include(s => s.User)
      .Where(s => s.EventId == eventId)
      .OrderBy(s => s.RegistrationDate)
      .ToListAsync();

  public async Task<IEnumerable<SignIn>> GetByUserAsync(Guid userId)
    => await context.SignIns
      .Include(s => s.Event)
      .Where(s => s.UserId == userId)
      .OrderByDescending(s => s.RegistrationDate)
      .ToListAsync();

  public async Task<SignIn?> GetFirstOnWaitingListAsync(Guid eventId)
    => await context.SignIns
      .Where(s => s.EventId == eventId && s.IsOnWaitingList)
      .OrderBy(s => s.WaitingListPosition)
      .FirstOrDefaultAsync();

  public async Task<int> GetWaitingListCountAsync(Guid eventId)
    => await context.SignIns
      .CountAsync(s => s.EventId == eventId && s.IsOnWaitingList);

  public async Task AddAsync(SignIn signIn)
    => await context.SignIns.AddAsync(signIn);

  public Task UpdateAsync(SignIn signIn)
  {
    context.SignIns.Update(signIn);
    return Task.CompletedTask;
  }

  public async Task DeleteAsync(Guid id)
  {
    SignIn? signIn = await GetByIdAsync(id);
    if (signIn != null)
      context.SignIns.Remove(signIn);
  }

  public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();
}