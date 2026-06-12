using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Article.Request;

/// <summary>
/// Données requises pour la création d'un article.
/// </summary>
public class CreateArticleRequestDto
{
  /// <summary>
  /// Le titre de l'article.
  /// </summary>
  [Required(ErrorMessage = "Le titre est obligatoire.")]
  [MaxLength(300, ErrorMessage = "Le titre ne peut pas dépasser 300 caractères.")]
  public required string Title { get; set; }

  /// <summary>
  /// Le contenu textuel de l'article.
  /// </summary>
  [Required(ErrorMessage = "Le contenu est obligatoire.")]
  public required string Content { get; set; }

  /// <summary>
  /// L'auteur de l'article.
  /// </summary>
  [Required(ErrorMessage = "L'auteur est obligatoire.")]
  [MaxLength(100, ErrorMessage = "L'auteur ne peut pas dépasser 100 caractères.")]
  public required string Author { get; set; }

  /// <summary>
  /// Les URLs des images associées à l'article.
  /// </summary>
  public List<string> ImageUrls { get; set; } = new();
}