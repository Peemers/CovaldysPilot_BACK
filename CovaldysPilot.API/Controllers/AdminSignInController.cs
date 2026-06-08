using CovaldysPilot.Application.DTOs.SignIn.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

//controller admin et user séparé pour éviter les erreurs d'annotation "Authorization"

[ApiController]
[Route("api/admin/signins")]
[Authorize(Roles = "Admin")]
public class AdminSignInController(
  ISignInService signInService,
  ILogger<AdminSignInController> logger) : ControllerBase
{
  [HttpPost("events/{eventId:guid}/users/{userId:guid}")]
  [EndpointSummary("Inscrire manuellement un membre à un événement")]
  public async Task<ActionResult<SignInResponseDto>> AdminRegister(Guid eventId, Guid userId)
  {
    logger.LogInformation("POST /api/admin/signins/events/{EventId}/users/{UserId}", eventId, userId);
    SignInResponseDto signIn = await signInService.AdminRegisterAsync(userId, eventId);
    return CreatedAtAction(nameof(AdminRegister), signIn);
  }

  [HttpDelete("{signInId:guid}")]
  [EndpointSummary("Désinscrire manuellement un membre")]
  public async Task<IActionResult> AdminUnregister(Guid signInId)
  {
    logger.LogInformation("DELETE /api/admin/signins/{SignInId}", signInId);
    await signInService.AdminUnregisterAsync(signInId);
    return NoContent();
  }
}