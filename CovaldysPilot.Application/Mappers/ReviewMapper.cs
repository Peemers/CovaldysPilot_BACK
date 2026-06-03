using CovaldysPilot.Application.DTOs.Review.Response;
using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Mappers;

public static class ReviewMapper
{
  public static ReviewResponseDto ToReviewResponseDto(this Review review)
  {
    return new ReviewResponseDto
    {
      Id = review.Id,
      Note = review.Note,
      Comment = review.Comment,
      UserId = review.UserId,
      UserPseudo = review.User?.Pseudo ?? string.Empty,
      EventId = review.EventId,
      CreatedAt = review.CreatedAt
    };
  }
}