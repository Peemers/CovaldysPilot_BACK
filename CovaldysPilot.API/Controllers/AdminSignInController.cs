using CovaldysPilot.Application.DTOs.SignIn.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
  #region AdminRegister
  /// <summary>
  /// Inscrit manuellement un membre à un événement de manière asynchrone (action administrateur).
  /// </summary>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <param name="userId">L'identifiant unique de l'utilisateur.</param>
  /// <returns>Le DTO de réponse contenant les informations de l'inscription créée.</returns>
  /// <response code="201">L'inscription a été créée avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPost("events/{eventId:guid}/users/{userId:guid}")]
  [EndpointSummary("Inscrire manuellement un membre à un événement")]
  [ProducesResponseType(typeof(SignInResponseDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<SignInResponseDto>> AdminRegister(Guid eventId, Guid userId)
  {
    logger.LogInformation("POST /api/admin/signins/events/{EventId}/users/{UserId}", eventId, userId);
    SignInResponseDto signIn = await signInService.AdminRegisterAsync(userId, eventId);
    return CreatedAtAction(nameof(AdminRegister), signIn);
  }
  #endregion

  #region AdminUnregister
  /// <summary>
  /// Désinscrit manuellement un membre de manière asynchrone (action administrateur).
  /// </summary>
  /// <param name="signInId">L'identifiant unique de l'inscription.</param>
  /// <returns>Un résultat vide indiquant la réussite de la désinscription.</returns>
  /// <response code="204">La désinscription a été effectuée avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpDelete("{signInId:guid}")]
  [EndpointSummary("Désinscrire manuellement un membre")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> AdminUnregister(Guid signInId)
  {
    logger.LogInformation("DELETE /api/admin/signins/{SignInId}", signInId);
    await signInService.AdminUnregisterAsync(signInId);
    return NoContent();
  }
  #endregion
}