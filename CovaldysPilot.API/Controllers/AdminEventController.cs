using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/admin/events")]
[Authorize(Roles = "Admin")]
public class AdminEventController(
  IEventService eventService,
  ILogger<AdminEventController> logger) : ControllerBase
{
  #region SendReminder
  /// <summary>
  /// Envoie un rappel à tous les inscrits d'un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement.</param>
  /// <returns>Un résultat vide indiquant que l'opération s'est déroulée avec succès.</returns>
  /// <response code="204">Le rappel a été envoyé avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPost("{id:guid}/reminder")]
  [EndpointSummary("Envoyer un rappel à tous les inscrits d'un événement")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> SendReminder(Guid id)
  {
    logger.LogInformation("POST /api/admin/events/{Id}/reminder", id);
    await eventService.SendReminderAsync(id);
    return NoContent();
  }
  #endregion
}
