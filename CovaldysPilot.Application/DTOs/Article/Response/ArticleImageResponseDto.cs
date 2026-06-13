namespace CovaldysPilot.Application.DTOs.Article.Response;

/// <summary>
/// Données de réponse représentant l'image d'un article.
/// </summary>
public class ArticleImageResponseDto
{
  /// <summary>
  /// L'identifiant unique de l'image.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// L'URL de l'image.
  /// </summary>
  public required string Url { get; init; }
}