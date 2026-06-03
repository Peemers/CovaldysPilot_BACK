using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace CovaldysPilot.Infrastructure.Repositories;

public class ReviewRepository(CovaldysPilotDbContext context) : IReviewRepository
{
  public async Task<Review?> GetByIdAsync(Guid id)
    => await context.Reviews.FindAsync(id);

  public async Task<IEnumerable<Review>> GetAllAsync()
    => await context.Reviews.ToListAsync();

  public async Task<IEnumerable<Review>> GetByEventAsync(Guid eventId)
    => await context.Reviews
      .Include(r => r.User)
      .Where(r => r.EventId == eventId)
      .OrderByDescending(r => r.CreatedAt)
      .ToListAsync();

  public async Task<Review?> GetByUserAndEventAsync(Guid userId, Guid eventId)
    => await context.Reviews
      .FirstOrDefaultAsync(r => r.UserId == userId && r.EventId == eventId);

  public async Task AddAsync(Review review)
    => await context.Reviews.AddAsync(review);

  public Task UpdateAsync(Review review)
  {
    context.Reviews.Update(review);
    return Task.CompletedTask;
  }

  public async Task DeleteAsync(Guid id)
  {
    Review? review = await GetByIdAsync(id);
    if (review != null)
      context.Reviews.Remove(review);
  }

  public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();
}