using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.Interfaces.Repositories;

public interface IEventRepository : IBaseRepository<Event>
{
  Task<IEnumerable<Event>> GetAllWithCategoriesAsync();
  Task<Event?> GetByIdWithDetailsAsync(Guid id);
  Task<IEnumerable<Event>> GetByStatusAsync(EventStatus status);
  Task<int> GetCurrentParticipantsCountAsync(Guid eventId);
  Task<bool> AnyByCategoryIdAsync(Guid categoryId);
}