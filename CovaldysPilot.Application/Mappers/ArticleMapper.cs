using CovaldysPilot.Application.DTOs.Article.Request;
using CovaldysPilot.Application.DTOs.Article.Response;
using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Mappers;

public static class ArticleMapper
{
  public static ArticleResponseDto ToArticleResponseDto(this Article article)
  {
    return new ArticleResponseDto
    {
      Id = article.Id,
      Title = article.Title,
      Content = article.Content,
      Author = article.Author,
      ViewCount = article.ViewCount,
      PublicationDate = article.PublicationDate,
      CreatedAt = article.CreatedAt,
      UpdatedAt = article.UpdatedAt,
      UserId = article.UserId,
      Images = article.Images
        .Select(i => new ArticleImageResponseDto { Id = i.Id, Url = i.Url })
        .ToList()
    };
  }

  public static Article ToArticle(this CreateArticleRequestDto dto, Guid? userId)
  {
    return new Article
    {
      Title = dto.Title,
      Content = dto.Content,
      Author = dto.Author,
      PublicationDate = DateTime.UtcNow,
      CreatedAt = DateTime.UtcNow,
      UserId = userId,
      Images = dto.ImageUrls.Take(2)
        .Select(url => new ArticleImage
        {
          Url = url,
          CreatedAt = DateTime.UtcNow,
          ArticleId = Guid.Empty
        }).ToList()
    };
  }

  public static void UpdateFromDto(this Article article, UpdateArticleRequestDto dto)
  {
    article.Title = dto.Title;
    article.Content = dto.Content;
    article.Author = dto.Author;
    article.UpdatedAt = DateTime.UtcNow;
  }
}