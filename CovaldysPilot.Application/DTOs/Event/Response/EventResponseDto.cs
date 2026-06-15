using CovaldysPilot.Application.DTOs.Category.Response;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.DTOs.Event.Response;

/// <summary>
/// Données de réponse représentant un événement.
/// </summary>
public class EventResponseDto
{
  /// <summary>
  /// L'identifiant unique de l'événement.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// Le nom de l'événement.
  /// </summary>
  public required string Name { get; init; }

  /// <summary>
  /// La description de l'événement.
  /// </summary>
  public required string Description { get; init; }

  /// <summary>
  /// Le lieu de l'événement.
  /// </summary>
  public string? Location { get; init; }

  /// <summary>
  /// L'URL de l'image de couverture de l'événement.
  /// </summary>
  public string? CoverImage { get; init; }

  /// <summary>
  /// La date et l'heure de début de l'événement.
  /// </summary>
  public DateTime StartDate { get; init; }

  /// <summary>
  /// La date et l'heure de fin de l'événement.
  /// </summary>
  public DateTime EndDate { get; init; }

  /// <summary>
  /// La date limite d'inscription à l'événement.
  /// </summary>
  public DateTime RegistrationDeadline { get; init; }

  /// <summary>
  /// Le nombre minimum de participants requis.
  /// </summary>
  public int MinParticipants { get; init; }

  /// <summary>
  /// Le nombre maximum de participants autorisés.
  /// </summary>
  public int MaxParticipants { get; init; }

  /// <summary>
  /// Le nombre actuel de participants inscrits.
  /// </summary>
  public int CurrentParticipants { get; init; }

  /// <summary>
  /// Le statut actuel de l'événement.
  /// </summary>
  public EventStatus Status { get; init; }

  /// <summary>
  /// Indique si la liste d'attente est active pour cet événement.
  /// </summary>
  public bool IsWaitingListActive { get; init; }

  /// <summary>
  /// La date et l'heure de création de l'événement.
  /// </summary>
  public DateTime CreatedAt { get; init; }

  /// <summary>
  /// La date et l'heure de la dernière mise à jour de l'événement.
  /// </summary>
  public DateTime? UpdatedAt { get; init; }

  /// <summary>
  /// Les catégories associées à l'événement.
  /// </summary>
  public List<CategoryResponseDto> Categories { get; init; } = new List<CategoryResponseDto>();

  /// <summary>
  /// Le prix d'entrée de l'événement.
  /// </summary>
  public decimal? Price { get; init; }

  /// <summary>
  /// L'identifiant unique de l'inscription de l'utilisateur connecté s'il est inscrit.
  /// </summary>
  public string? SignInId { get; init; }

  // Champs calculés

  /// <summary>
  /// Indique si l'utilisateur actuellement connecté peut s'inscrire à cet événement.
  /// </summary>
  public bool CanRegister { get; init; }

  /// <summary>
  /// Indique si l'utilisateur actuellement connecté est déjà inscrit à cet événement.
  /// </summary>
  public bool IsRegistered { get; init; }

  /// <summary>
  /// La position de l'utilisateur sur la liste d'attente s'il y figure.
  /// </summary>
  public int? WaitingListPosition { get; init; }

  /// <summary>
  /// Indique si l'utilisateur est sur la liste d'attente pour cet événement.
  /// </summary>
  public bool IsOnWaitingList { get; init; }
}