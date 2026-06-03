using CovaldysPilot.Application.DTOs.Review.Request;
using CovaldysPilot.Application.DTOs.Review.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IReviewService
{
  Task<IEnumerable<ReviewResponseDto>> GetByEventAsync(Guid eventId);
  Task<ReviewResponseDto> CreateAsync(Guid userId, CreateReviewRequestDto dto);
  Task<ReviewResponseDto> UpdateAsync(Guid userId, Guid reviewId, UpdateReviewRequestDto dto);
  Task DeleteAsync(Guid userId, Guid reviewId);
}