namespace CovaldysPilot.Application.DTOs.Review.Response;

/// <summary>
/// Données de réponse représentant un avis.
/// </summary>
public class ReviewResponseDto
{
  /// <summary>
  /// L'identifiant unique de l'avis.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// La note attribuée à l'événement.
  /// </summary>
  public int Note { get; init; }

  /// <summary>
  /// Le commentaire rédigé pour l'événement.
  /// </summary>
  public string? Comment { get; init; }

  /// <summary>
  /// L'identifiant unique de l'utilisateur ayant rédigé l'avis.
  /// </summary>
  public Guid UserId { get; init; }

  /// <summary>
  /// Le pseudonyme de l'utilisateur ayant rédigé l'avis.
  /// </summary>
  public string UserPseudo { get; init; } = string.Empty;

  /// <summary>
  /// L'identifiant unique de l'événement évalué.
  /// </summary>
  public Guid EventId { get; init; }

  /// <summary>
  /// La date et l'heure de création de l'avis.
  /// </summary>
  public DateTime CreatedAt { get; init; }
}