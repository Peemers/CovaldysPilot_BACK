using CovaldysPilot.Application.DTOs.SiteConfiguration.Request;
using CovaldysPilot.Application.DTOs.SiteConfiguration.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/admin/config")]
[Authorize(Roles = "Admin")]
public class AdminSiteConfigurationController(
  ISiteConfigurationService siteConfigurationService,
  ILogger<AdminSiteConfigurationController> logger) : ControllerBase
{
  [HttpGet]
  [AllowAnonymous]
  [EndpointSummary("Récupérer la configuration du site")]
  public async Task<ActionResult<SiteConfigurationResponseDto>> Get()
  {
    logger.LogInformation("GET /api/admin/config");
    SiteConfigurationResponseDto config = await siteConfigurationService.GetAsync();
    return Ok(config);
  }

  [HttpPatch("maintenance")]
  [EndpointSummary("Activer/désactiver le mode maintenance")]
  public async Task<ActionResult<SiteConfigurationResponseDto>> UpdateMaintenance(
    [FromBody] UpdateMaintanceRequestDto dto)
  {
    logger.LogInformation("PATCH /api/admin/config/maintenance - {IsMaintenanceMode}", dto.IsMaintenanceMode);
    SiteConfigurationResponseDto config = await siteConfigurationService.UpdateMaintenanceAsync(dto);
    return Ok(config);
  }

  [HttpPatch("alert")]
  [EndpointSummary("Modifier le message d'alerte global")]
  public async Task<ActionResult<SiteConfigurationResponseDto>> UpdateAlert(
    [FromBody] UpdateAlertRequestDto dto)
  {
    logger.LogInformation("PATCH /api/admin/config/alert");
    SiteConfigurationResponseDto config = await siteConfigurationService.UpdateAlertMessageAsync(dto);
    return Ok(config);
  }
}