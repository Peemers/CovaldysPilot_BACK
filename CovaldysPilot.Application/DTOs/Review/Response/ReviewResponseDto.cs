namespace CovaldysPilot.Application.DTOs.Review.Response;

public class ReviewResponseDto
{
  public Guid Id { get; set; }
  public int Note { get; set; }
  public string? Comment { get; set; }
  public Guid UserId { get; set; }
  public string UserPseudo { get; set; } = string.Empty;
  public Guid EventId { get; set; }
  public DateTime CreatedAt { get; set; }
}