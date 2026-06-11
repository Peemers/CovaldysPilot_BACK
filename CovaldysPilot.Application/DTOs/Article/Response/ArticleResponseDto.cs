namespace CovaldysPilot.Application.DTOs.Article.Response;

public class ArticleResponseDto
{
  public Guid Id { get; init; }
  public required string Title { get; init; }
  public required string Content { get; init; }
  public string Author { get; init; } = string.Empty;
  public int ViewCount { get; init; }
  public DateTime PublicationDate { get; init; }
  public DateTime CreatedAt { get; init; }
  public DateTime? UpdatedAt { get; init; }
  public Guid? UserId { get; init; }
  public List<ArticleImageResponseDto> Images { get; init; } = new();
}