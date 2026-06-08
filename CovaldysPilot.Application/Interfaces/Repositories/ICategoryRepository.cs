using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
  Task<bool> NameExistsAsync(string name);
  Task<Category?> GetByNameAsync(string name);
}