namespace CovaldysPilot.Application.DTOs.SiteConfiguration.Response;

/// <summary>
/// Données de réponse représentant la configuration globale du site.
/// </summary>
public class SiteConfigurationResponseDto
{
  /// <summary>
  /// Indique si le mode maintenance est activé.
  /// </summary>
  public bool IsMaintenanceMode { get; init; }

  /// <summary>
  /// Le message d'alerte global affiché sur le site.
  /// </summary>
  public string? GlobalAlertMessage { get; init; }
}