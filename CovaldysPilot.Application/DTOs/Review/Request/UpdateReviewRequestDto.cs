namespace CovaldysPilot.Application.DTOs.Review.Request;

public class UpdateReviewRequestDto
{
  public required int Note { get; set; }
  public string? Comment { get; set; }
}