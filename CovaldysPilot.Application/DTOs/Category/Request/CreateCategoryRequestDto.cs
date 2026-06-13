using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Category.Request;

/// <summary>
/// Données requises pour la création d'une catégorie.
/// </summary>
public class CreateCategoryRequestDto
{
  /// <summary>
  /// Le nom de la catégorie.
  /// </summary>
  [Required]
  [StringLength(100, MinimumLength = 2,  ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères.")]
  public required string Name { get; set; }
}