using CovaldysPilot.Application.DTOs.Event.Request;
using CovaldysPilot.Application.DTOs.Event.Response;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.Mappers;

public static class EventMapper
{
  public static EventResponseDto ToEventResponseDto(
    this Event evt,
    int currentParticipants = 0,
    bool canRegister = false,
    bool isRegistered = false,
    int? waitingListPosition = null)
  {
    return new EventResponseDto
    {
      Id = evt.Id,
      Name = evt.Name,
      Description = evt.Description,
      Location = evt.Location,
      CoverImage = evt.CoverImage,
      StartDate = evt.StartDate,
      EndDate = evt.EndDate,
      RegistrationDeadline = evt.RegistrationDeadline,
      MinParticipants = evt.MinParticipants,
      MaxParticipants = evt.MaxParticipants,
      CurrentParticipants = currentParticipants,
      Status = evt.Status,
      IsWaitingListActive = evt.IsWaitingListActive,
      CreatedAt = evt.CreatedAt,
      UpdatedAt = evt.UpdatedAt,
      Categories = evt.EventCategories
        .Select(ec => ec.Category.ToCategoryResponseDto())
        .ToList(),
      CanRegister = canRegister,
      IsRegistered = isRegistered,
      WaitingListPosition = waitingListPosition
    };
  }

  public static Event ToEvent(this CreateEventRequestDto dto)
  {
    return new Event
    {
      Name = dto.Name,
      Description = dto.Description,
      Location = dto.Location,
      CoverImage = dto.CoverImage,
      StartDate = dto.StartDate,
      EndDate = dto.EndDate,
      RegistrationDeadline = dto.RegistrationDeadline,
      MinParticipants = dto.MinParticipants,
      MaxParticipants = dto.MaxParticipants,
      IsWaitingListActive = dto.IsWaitingListActive,
      Status = EventStatus.EnAttente,
      CreatedAt = DateTime.UtcNow
    };
  }
}