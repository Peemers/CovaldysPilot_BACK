namespace CovaldysPilot.Application.DTOs.Article.Request;

public class UpdateArticleRequestDto
{
  public required string Title { get; set; }
  public required string Content { get; set; }
  public required string Author { get; set; }
  public List<string> ImageUrls { get; set; } = new();
}