using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Event.Request;

/// <summary>
/// Données requises pour la mise à jour d'un événement.
/// </summary>
public class UpdateEventRequestDto
{
  /// <summary>
  /// Le nom de l'événement.
  /// </summary>
  [Required(ErrorMessage = "Le nom est obligatoire.")]
  [StringLength(200, MinimumLength = 3)]
  public required string Name { get; set; }

  /// <summary>
  /// La description de l'événement.
  /// </summary>
  [Required(ErrorMessage = "La description est obligatoire.")]
  public required string Description { get; set; }
  
  /// <summary>
  /// Le prix d'entrée à l'événement.
  /// </summary>
  [Range(0, 10000)]
  public decimal? Price { get; set; }
  
  /// <summary>
  /// Le lieu de l'événement.
  /// </summary>
  [MaxLength(300, ErrorMessage = "Le lieu ne peut pas dépasser 300 caractères.")]
  public string? Location { get; set; }

  /// <summary>
  /// L'URL de l'image de couverture de l'événement.
  /// </summary>
  [MaxLength(500, ErrorMessage = "L'URL de l'image de couverture ne peut pas dépasser 500 caractères.")]
  public string? CoverImage { get; set; }

  /// <summary>
  /// La date et l'heure de début de l'événement.
  /// </summary>
  [Required(ErrorMessage = "La date de début est obligatoire.")]
  public required DateTime StartDate { get; set; }

  /// <summary>
  /// La date et l'heure de fin de l'événement.
  /// </summary>
  [Required(ErrorMessage = "La date de fin est obligatoire.")]
  public required DateTime EndDate { get; set; }

  /// <summary>
  /// La date limite d'inscription à l'événement.
  /// </summary>
  [Required(ErrorMessage = "La date limite d'inscription est obligatoire.")]
  public required DateTime RegistrationDeadline { get; set; }

  /// <summary>
  /// Le nombre minimum de participants requis.
  /// </summary>
  [Range(1, 200)]
  public int MinParticipants { get; set; }

  /// <summary>
  /// Le nombre maximum de participants autorisés.
  /// </summary>
  [Range(1, 200)]
  public int MaxParticipants { get; set; }

  /// <summary>
  /// Indique si la liste d'attente est active pour cet événement.
  /// </summary>
  public bool IsWaitingListActive { get; set; }

  /// <summary>
  /// Les identifiants uniques des catégories associées à l'événement.
  /// </summary>
  public List<Guid> CategoryIds { get; set; } = new List<Guid>();
}