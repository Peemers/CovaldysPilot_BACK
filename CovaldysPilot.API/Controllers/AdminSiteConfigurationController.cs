using CovaldysPilot.Application.DTOs.SiteConfiguration.Request;
using CovaldysPilot.Application.DTOs.SiteConfiguration.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/admin/config")]
[Authorize(Roles = "Admin")]
public class AdminSiteConfigurationController(
  ISiteConfigurationService siteConfigurationService,
  ILogger<AdminSiteConfigurationController> logger) : ControllerBase
{
  #region Get
  /// <summary>
  /// Récupère la configuration globale du site de manière asynchrone.
  /// </summary>
  /// <returns>Le DTO de réponse contenant la configuration actuelle du site.</returns>
  /// <response code="200">La configuration du site a été récupérée avec succès.</response>
  [HttpGet]
  [AllowAnonymous]
  [EndpointSummary("Récupérer la configuration du site")]
  [ProducesResponseType(typeof(SiteConfigurationResponseDto), StatusCodes.Status200OK)]
  public async Task<ActionResult<SiteConfigurationResponseDto>> Get()
  {
    logger.LogInformation("GET /api/admin/config");
    SiteConfigurationResponseDto config = await siteConfigurationService.GetAsync();
    return Ok(config);
  }
  #endregion

  #region UpdateMaintenance
  /// <summary>
  /// Active ou désactive le mode de maintenance du site de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant l'état de maintenance à appliquer.</param>
  /// <returns>Le DTO de réponse contenant la configuration du site mise à jour.</returns>
  /// <response code="200">La maintenance a été mise à jour avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPatch("maintenance")]
  [EndpointSummary("Activer/désactiver le mode maintenance")]
  [ProducesResponseType(typeof(SiteConfigurationResponseDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<SiteConfigurationResponseDto>> UpdateMaintenance(
    [FromBody] UpdateMaintanceRequestDto dto)
  {
    logger.LogInformation("PATCH /api/admin/config/maintenance - {IsMaintenanceMode}", dto.IsMaintenanceMode);
    SiteConfigurationResponseDto config = await siteConfigurationService.UpdateMaintenanceAsync(dto);
    return Ok(config);
  }
  #endregion

  #region UpdateAlert
  /// <summary>
  /// Modifie le message d'alerte global affiché sur le site de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant le nouveau message d'alerte et son état d'activation.</param>
  /// <returns>Le DTO de réponse contenant la configuration du site mise à jour.</returns>
  /// <response code="200">Le message d'alerte a été mis à jour avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPatch("alert")]
  [EndpointSummary("Modifier le message d'alerte global")]
  [ProducesResponseType(typeof(SiteConfigurationResponseDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<SiteConfigurationResponseDto>> UpdateAlert(
    [FromBody] UpdateAlertRequestDto dto)
  {
    logger.LogInformation("PATCH /api/admin/config/alert");
    SiteConfigurationResponseDto config = await siteConfigurationService.UpdateAlertMessageAsync(dto);
    return Ok(config);
  }
  #endregion
}