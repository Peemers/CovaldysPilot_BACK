namespace CovaldysPilot.Application.DTOs.Review.Response;

public class ReviewResponseDto
{
  public Guid Id { get; init; }
  public int Note { get; init; }
  public string? Comment { get; init; }
  public Guid UserId { get; init; }
  public string UserPseudo { get; init; } = string.Empty;
  public Guid EventId { get; init; }
  public DateTime CreatedAt { get; init; }
}