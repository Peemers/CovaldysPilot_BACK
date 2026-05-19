namespace CovaldysPilot.Domain.Entities;

public class Article : BaseEntity
{
  public required string Title { get; set; } = string.Empty;
  public required string Content { get; set; } = string.Empty;
  public string Author { get; set; } = string.Empty;
  public int ViewCount { get; set; } = 0;
  public DateTime PublicationDate { get; set; }

  public required Guid UserId { get; set; }
  public User User { get; set; } = null!;

  public ICollection<ArticleImage> Images { get; set; } = new List<ArticleImage>();
}