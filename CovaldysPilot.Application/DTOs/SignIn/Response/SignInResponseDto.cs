using CovaldysPilot.Application.DTOs.Event.Response;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.DTOs.SignIn.Response;

/// <summary>
/// Données de réponse représentant une inscription à un événement.
/// </summary>
public class SignInResponseDto
{
  /// <summary>
  /// L'identifiant unique de l'inscription.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// L'identifiant unique de l'événement.
  /// </summary>
  public Guid EventId { get; init; }

  /// <summary>
  /// L'identifiant unique du membre inscrit.
  /// </summary>
  public Guid UserId { get; init; }

  /// <summary>
  /// La date et l'heure d'inscription.
  /// </summary>
  public DateTime RegistrationDate { get; init; }

  /// <summary>
  /// Indique si l'utilisateur est sur la liste d'attente.
  /// </summary>
  public bool IsOnWaitingList { get; init; }

  /// <summary>
  /// La position du membre sur la liste d'attente s'il y figure.
  /// </summary>
  public int? WaitingListPosition { get; init; }

  /// <summary>
  /// Indique si le paiement associé à l'inscription a été validé.
  /// </summary>
  public bool IsPaymentValid { get; init; }

  /// <summary>
  /// Le pseudonyme du membre inscrit.
  /// </summary>
  public string? UserPseudo { get; init; }

  /// <summary>
  /// Le prénom du membre inscrit.
  /// </summary>
  public string? UserFirstName { get; init; }

  /// <summary>
  /// Le nom de famille du membre inscrit.
  /// </summary>
  public string? UserLastName { get; init; }

  /// <summary>
  /// Le nom de l'événement.
  /// </summary>
  public string? EventName { get; init; }

  /// <summary>
  /// Le statut actuel de l'événement.
  /// </summary>
  public EventStatus? EventStatus { get; init; }

  /// <summary>
  /// La date et l'heure de début de l'événement.
  /// </summary>
  public DateTime? EventStartDate { get; init; }
}