namespace CovaldysPilot.Application.DTOs.Article.Response;

public class ArticleResponseDto
{
  public Guid Id { get; set; }
  public required string Title { get; set; }
  public required string Content { get; set; }
  public string Author { get; set; } = string.Empty;
  public int ViewCount { get; set; }
  public DateTime PublicationDate { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public Guid? UserId { get; set; }
  public List<ArticleImageResponseDto> Images { get; set; } = new();
}