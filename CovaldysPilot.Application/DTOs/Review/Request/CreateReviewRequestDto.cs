using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Review.Request;

/// <summary>
/// Données requises pour laisser un avis sur un événement.
/// </summary>
public class CreateReviewRequestDto
{
  /// <summary>
  /// L'identifiant unique de l'événement.
  /// </summary>
  [Required(ErrorMessage = "L'identifiant de l'événement est obligatoire.")]
  public Guid EventId { get; set; }

  /// <summary>
  /// La note attribuée à l'événement (entre 1 et 5).
  /// </summary>
  [Required(ErrorMessage = "La note est obligatoire.")]
  [Range(1, 5, ErrorMessage = "La note doit être comprise entre 1 et 5.")]
  public required int Note { get; set; }

  /// <summary>
  /// Le commentaire de l'avis.
  /// </summary>
  public string? Comment { get; set; }
}