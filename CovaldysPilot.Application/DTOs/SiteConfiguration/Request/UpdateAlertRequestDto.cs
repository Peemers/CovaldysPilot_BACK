namespace CovaldysPilot.Application.DTOs.SiteConfiguration.Request;

/// <summary>
/// Données de requête pour mettre à jour le message d'alerte global.
/// </summary>
public class UpdateAlertRequestDto
{
  /// <summary>
  /// Le message d'alerte global.
  /// </summary>
  public string? GlobalAlertMessage { get; set; }
}