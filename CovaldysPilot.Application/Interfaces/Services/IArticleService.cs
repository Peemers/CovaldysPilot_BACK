using CovaldysPilot.Application.DTOs.Article.Request;
using CovaldysPilot.Application.DTOs.Article.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IArticleService
{
  Task<IEnumerable<ArticleResponseDto>> GetAllAsync();
  Task<ArticleResponseDto?> GetByIdAsync(Guid id);
  Task<ArticleResponseDto> CreateAsync(CreateArticleRequestDto dto, Guid? userId);
  Task<ArticleResponseDto> UpdateAsync(Guid id, UpdateArticleRequestDto dto);
  Task DeleteAsync(Guid id);
  Task<ArticleResponseDto> AddImageAsync(Guid id, string imageUrl);
  Task DeleteImageAsync(Guid articleId, Guid imageId);
}