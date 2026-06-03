using CovaldysPilot.Application.DTOs.Event.Request;
using CovaldysPilot.Application.DTOs.Event.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IEventService
{
  Task<IEnumerable<EventResponseDto>> GetAllAsync(Guid? currentUserId = null);
  Task<EventResponseDto?> GetByIdAsync(Guid id, Guid? currentUserId = null);
  Task<EventResponseDto> CreateAsync(CreateEventRequestDto dto);
  Task<EventResponseDto> UpdateAsync(Guid id, UpdateEventRequestDto dto);
  Task DeleteAsync(Guid id);
  Task CancelAsync(Guid id, string? cancellationReason = null);
  Task StartAsync(Guid id);
  Task CloseAsync(Guid id);
  Task<EventStatsResponseDto> GetStatsAsync(Guid id);
  Task SendReminderAsync(Guid id);
}