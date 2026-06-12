using CovaldysPilot.Application.DTOs.Category.Response;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.DTOs.Event.Response;

public class EventResponseDto
{
  public Guid Id { get; init; }
  public required string Name { get; init; }
  public required string Description { get; init; }
  public string? Location { get; init; }
  public string? CoverImage { get; init; }
  public DateTime StartDate { get; init; }
  public DateTime EndDate { get; init; }
  public DateTime RegistrationDeadline { get; init; }
  public int MinParticipants { get; init; }
  public int MaxParticipants { get; init; }
  public int CurrentParticipants { get; init; }
  public EventStatus Status { get; init; }
  public bool IsWaitingListActive { get; init; }
  public DateTime CreatedAt { get; init; }
  public DateTime? UpdatedAt { get; init; }
  public List<CategoryResponseDto> Categories { get; set; } = new List<CategoryResponseDto>();
  public decimal? Price { get; init; }
  public string? SignInId { get; init; }

  // Champs calculés
  public bool CanRegister { get; init; }
  public bool IsRegistered { get; init; }
  public int? WaitingListPosition { get; init; }
}