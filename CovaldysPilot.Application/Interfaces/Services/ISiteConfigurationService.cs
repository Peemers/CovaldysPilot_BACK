using CovaldysPilot.Application.DTOs.SiteConfiguration.Request;
using CovaldysPilot.Application.DTOs.SiteConfiguration.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface ISiteConfigurationService
{
  #region GetAsync
  /// <summary>
  /// Récupère la configuration actuelle du site de manière asynchrone.
  /// </summary>
  /// <returns>Le DTO de réponse contenant les détails de la configuration du site.</returns>
  Task<SiteConfigurationResponseDto> GetAsync();
  #endregion

  #region UpdateMaintenanceAsync
  /// <summary>
  /// Met à jour l'état de maintenance du site de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les données de mise à jour de la maintenance.</param>
  /// <returns>Le DTO de réponse contenant la configuration du site mise à jour.</returns>
  Task<SiteConfigurationResponseDto> UpdateMaintenanceAsync(UpdateMaintanceRequestDto dto);
  #endregion

  #region UpdateAlertMessageAsync
  /// <summary>
  /// Met à jour le message d'alerte du site de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les données de mise à jour du message d'alerte.</param>
  /// <returns>Le DTO de réponse contenant la configuration du site mise à jour.</returns>
  Task<SiteConfigurationResponseDto> UpdateAlertMessageAsync(UpdateAlertRequestDto dto);
  #endregion
}