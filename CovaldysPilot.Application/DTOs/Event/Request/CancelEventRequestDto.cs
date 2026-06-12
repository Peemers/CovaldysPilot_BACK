namespace CovaldysPilot.Application.DTOs.Event.Request;

/// <summary>
/// Données requises pour l'annulation d'un événement.
/// </summary>
public class CancelEventRequestDto
{
  /// <summary>
  /// Le motif d'annulation de l'événement.
  /// </summary>
  public string? CancellationReason { get; set; }
}