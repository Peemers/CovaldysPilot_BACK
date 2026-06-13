using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Review.Request;

/// <summary>
/// Données requises pour modifier un avis existant.
/// </summary>
public class UpdateReviewRequestDto
{
  /// <summary>
  /// La nouvelle note attribuée à l'événement (entre 1 et 5).
  /// </summary>
  [Required(ErrorMessage = "La note est obligatoire.")]
  [Range(1, 5, ErrorMessage = "La note doit être comprise entre 1 et 5.")]
  public required int Note { get; set; }

  /// <summary>
  /// Le nouveau commentaire de l'avis.
  /// </summary>
  public string? Comment { get; set; }
}