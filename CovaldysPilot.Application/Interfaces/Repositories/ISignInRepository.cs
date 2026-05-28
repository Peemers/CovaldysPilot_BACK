using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

public interface ISignInRepository : IBaseRepository<SignIn>
{
  Task<SignIn?> GetByUserAndEventAsync(Guid userId, Guid eventId);
  Task<IEnumerable<SignIn>> GetByEventAsync(Guid eventId);
  Task<IEnumerable<SignIn>> GetByUserAsync(Guid userId);
  Task<SignIn?> GetFirstOnWaitingListAsync(Guid eventId);
  Task<int> GetWaitingListCountAsync(Guid eventId);
}