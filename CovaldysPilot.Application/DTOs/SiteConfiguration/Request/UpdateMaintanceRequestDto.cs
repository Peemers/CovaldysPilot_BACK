using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.SiteConfiguration.Request;

/// <summary>
/// Données de requête pour mettre à jour l'état du mode maintenance.
/// </summary>
public class UpdateMaintanceRequestDto
{
  /// <summary>
  /// Indique si le mode maintenance est activé.
  /// </summary>
  [Required]
  public required bool IsMaintenanceMode { get; set; }
}