using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Domain.Entities;

public class Event : BaseEntity
{
  public required string Name { get; set; } = string.Empty;
  public required string Description { get; set; } = string.Empty;
  public string? Location { get; set; }
  public string? CoverImage { get; set; }
  public required DateTime StartDate { get; set; }
  public required DateTime EndDate { get; set; }
  public required DateTime RegistrationDeadline { get; set; }
  public int MinParticipants { get; set; }
  public int MaxParticipants { get; set; }
  public EventStatus Status { get; set; } = EventStatus.EnAttente;
  public bool IsWaitingListActive { get; set; }

  public ICollection<SignIn> SignIns { get; set; } = new List<SignIn>();
  public ICollection<Review> Reviews { get; set; } = new List<Review>();
  public ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();
}