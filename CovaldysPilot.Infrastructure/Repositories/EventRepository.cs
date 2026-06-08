using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;
using CovaldysPilot.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace CovaldysPilot.Infrastructure.Repositories;

public class EventRepository(CovaldysPilotDbContext context) : IEventRepository
{
  public async Task<Event?> GetByIdAsync(Guid id)
    => await context.Events.FindAsync(id);

  public async Task<IEnumerable<Event>> GetAllAsync()
    => await context.Events.ToListAsync();

  public async Task<IEnumerable<Event>> GetAllWithCategoriesAsync()
    => await context.Events
      .Include(e => e.EventCategories)
      .ThenInclude(ec => ec.Category)
      .Include(e => e.SignIns)
      .OrderByDescending(e => e.UpdatedAt ?? e.CreatedAt)
      .ToListAsync();

  public async Task<Event?> GetByIdWithDetailsAsync(Guid id)
    => await context.Events
      .Include(e => e.EventCategories)
      .ThenInclude(ec => ec.Category)
      .Include(e => e.SignIns)
      .FirstOrDefaultAsync(e => e.Id == id);

  public async Task<IEnumerable<Event>> GetByStatusAsync(EventStatus status)
    => await context.Events
      .Include(e => e.EventCategories)
      .ThenInclude(ec => ec.Category)
      .Where(e => e.Status == status)
      .ToListAsync();

  public async Task<int> GetCurrentParticipantsCountAsync(Guid eventId)
    => await context.SignIns
      .CountAsync(s => s.EventId == eventId && !s.IsOnWaitingList);

  public async Task AddAsync(Event evt)
    => await context.Events.AddAsync(evt);

  public Task UpdateAsync(Event evt)
  {
    context.Events.Update(evt);
    return Task.CompletedTask;
  }

  public async Task DeleteAsync(Guid id)
  {
    Event? evt = await GetByIdAsync(id);
    if (evt != null)
      context.Events.Remove(evt);
  }

  public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();
}