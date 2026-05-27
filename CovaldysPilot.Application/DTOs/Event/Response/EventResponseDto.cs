using CovaldysPilot.Application.DTOs.Category.Response;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.DTOs.Event.Response;

public class EventResponseDto
{
  public Guid Id { get; set; }
  public required string Name { get; set; }
  public required string Description { get; set; }
  public string? Location { get; set; }
  public string? CoverImage { get; set; }
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public DateTime RegistrationDeadline { get; set; }
  public int MinParticipants { get; set; }
  public int MaxParticipants { get; set; }
  public int CurrentParticipants { get; set; }
  public EventStatus Status { get; set; }
  public bool IsWaitingListActive { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public List<CategoryResponseDto> Categories { get; set; } = new List<CategoryResponseDto>();
  public decimal? Price { get; set; }

  // Champs calculés
  public bool CanRegister { get; set; }
  public bool IsRegistered { get; set; }
  public int? WaitingListPosition { get; set; }
}