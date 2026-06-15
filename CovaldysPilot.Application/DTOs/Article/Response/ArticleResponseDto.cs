namespace CovaldysPilot.Application.DTOs.Article.Response;

/// <summary>
/// Données de réponse représentant un article.
/// </summary>
public class ArticleResponseDto
{
  /// <summary>
  /// L'identifiant unique de l'article.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// Le titre de l'article.
  /// </summary>
  public required string Title { get; init; }

  /// <summary>
  /// Le contenu textuel de l'article.
  /// </summary>
  public required string Content { get; init; }

  /// <summary>
  /// L'auteur de l'article.
  /// </summary>
  public string Author { get; init; } = string.Empty;

  /// <summary>
  /// Le nombre de vues de l'article.
  /// </summary>
  public int ViewCount { get; init; }

  /// <summary>
  /// La date de publication de l'article.
  /// </summary>
  public DateTime PublicationDate { get; init; }

  /// <summary>
  /// La date et l'heure de création de l'article.
  /// </summary>
  public DateTime CreatedAt { get; init; }

  /// <summary>
  /// La date et l'heure de la dernière mise à jour de l'article.
  /// </summary>
  public DateTime? UpdatedAt { get; init; }

  /// <summary>
  /// L'identifiant unique de l'utilisateur ayant créé l'article.
  /// </summary>
  public Guid? UserId { get; init; }

  /// <summary>
  /// Les images associées à l'article.
  /// </summary>
  public List<ArticleImageResponseDto> Images { get; init; } = new();
}