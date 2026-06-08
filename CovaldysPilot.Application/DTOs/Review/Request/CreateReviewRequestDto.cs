namespace CovaldysPilot.Application.DTOs.Review.Request;

public class CreateReviewRequestDto
{
  public Guid EventId { get; set; }
  public required int Note { get; set; }
  public string? Comment { get; set; }
}