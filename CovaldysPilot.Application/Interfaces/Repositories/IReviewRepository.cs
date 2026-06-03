using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

public interface IReviewRepository : IBaseRepository<Review>
{
  Task<IEnumerable<Review>> GetByEventAsync(Guid eventId);
  Task<Review?> GetByUserAndEventAsync(Guid userId, Guid eventId);
}