namespace CovaldysPilot.Domain.Entities;

public class Review : BaseEntity
{
  public required int Note { get; set; }
  public string? Comment { get; set; }

  public required Guid UserId { get; set; }
  public User User { get; set; } = null!;

  public required Guid EventId { get; set; }
  public Event Event { get; set; } = null!;
}