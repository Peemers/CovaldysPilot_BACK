namespace CovaldysPilot.Domain.Entities;

public class ArticleImage : BaseEntity
{
  public required string Url { get; set; } = string.Empty;
  public required Guid ArticleId { get; set; }
  public Article Article { get; set; } = null!;
}