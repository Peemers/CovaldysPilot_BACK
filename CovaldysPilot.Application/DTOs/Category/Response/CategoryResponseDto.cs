namespace CovaldysPilot.Application.DTOs.Category.Response;

/// <summary>
/// Données de réponse représentant une catégorie.
/// </summary>
public class CategoryResponseDto
{
  /// <summary>
  /// L'identifiant unique de la catégorie.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// Le nom de la catégorie.
  /// </summary>
  public required string Name { get; init; }

  /// <summary>
  /// La date et l'heure de création de la catégorie.
  /// </summary>
  public DateTime CreatedAt { get; init; }
}