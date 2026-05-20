using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
  Task<User?> GetByEmailAsync(string email);
  Task<User?> GetByEmailOrPseudoAsync(string emailOrPseudo);
  Task<bool> EmailExistsAsync(string email);
  Task<bool> PseudoExistsAsync(string pseudo);
}