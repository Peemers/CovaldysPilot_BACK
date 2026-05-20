using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace CovaldysPilot.Infrastructure.Repositories;

public class UserRepository(CovaldysPilotDbContext context) : IUserRepository
{
  public async Task<User?> GetByIdAsync(Guid id)
    => await context.Users.FindAsync(id);

  public async Task<IEnumerable<User>> GetAllAsync()
    => await context.Users.ToListAsync();

  public async Task<User?> GetByEmailAsync(string email)
    => await context.Users.FirstOrDefaultAsync(u => u.Email == email);

  public async Task<User?> GetByPseudoAsync(string pseudo)
    => await context.Users.FirstOrDefaultAsync(u => u.Pseudo == pseudo);

  public async Task<User?> GetByEmailOrPseudoAsync(string emailOrPseudo)
    => await context.Users.FirstOrDefaultAsync(u => 
      u.Email == emailOrPseudo || u.Pseudo == emailOrPseudo);

  public async Task<bool> EmailExistsAsync(string email)
    => await context.Users.AnyAsync(u => u.Email == email);

  public async Task<bool> PseudoExistsAsync(string pseudo)
    => await context.Users.AnyAsync(u => u.Pseudo == pseudo);

  public async Task AddAsync(User user)
    => await context.Users.AddAsync(user);

  public Task UpdateAsync(User user)
  {
    context.Users.Update(user);
    return Task.CompletedTask;
  }

  public async Task DeleteAsync(Guid id)
  {
    var user = await GetByIdAsync(id);
    if (user != null)
      context.Users.Remove(user);
  }

  public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();
}