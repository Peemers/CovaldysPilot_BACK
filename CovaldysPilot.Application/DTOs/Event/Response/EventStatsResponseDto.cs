namespace CovaldysPilot.Application.DTOs.Event.Response;

/// <summary>
/// Données de réponse représentant les statistiques d'un événement.
/// </summary>
public class EventStatsResponseDto
{
  /// <summary>
  /// L'identifiant unique de l'événement.
  /// </summary>
  public Guid EventId { get; init; }

  /// <summary>
  /// Le nom de l'événement.
  /// </summary>
  public string EventName { get; init; } = string.Empty;

  /// <summary>
  /// Le nombre de participants confirmés.
  /// </summary>
  public int ConfirmedParticipants { get; init; }

  /// <summary>
  /// Le nombre de personnes sur la liste d'attente.
  /// </summary>
  public int WaitingListCount { get; init; }

  /// <summary>
  /// Le nombre maximum de participants autorisés.
  /// </summary>
  public int MaxParticipants { get; init; }

  /// <summary>
  /// Le taux de remplissage de l'événement en pourcentage (%).
  /// </summary>
  public double FillRate { get; init; } // en %
}