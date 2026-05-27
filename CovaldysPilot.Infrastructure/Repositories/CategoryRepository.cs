using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace CovaldysPilot.Infrastructure.Repositories;

public class CategoryRepository(CovaldysPilotDbContext context): ICategoryRepository
{
  public async Task<Category?> GetByIdAsync(Guid id)
    => await context.Categories.FindAsync(id);

  public async Task<IEnumerable<Category>> GetAllAsync()
    => await context.Categories.OrderBy(c => c.Name).ToListAsync();

  public async Task<Category?> GetByNameAsync(string name)
    => await context.Categories.FirstOrDefaultAsync(c => c.Name == name);

  public async Task<bool> NameExistsAsync(string name)
    => await context.Categories.AnyAsync(c => c.Name == name);

  public async Task AddAsync(Category category)
    => await context.Categories.AddAsync(category);

  public Task UpdateAsync(Category category)
  {
    context.Categories.Update(category);
    return Task.CompletedTask;
  }

  public async Task DeleteAsync(Guid id)
  {
    Category? category = await GetByIdAsync(id);
    if (category != null)
      context.Categories.Remove(category);
  }

  public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();
}