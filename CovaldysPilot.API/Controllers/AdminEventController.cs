using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/admin/events")]
[Authorize(Roles = "Admin")]
public class AdminEventController(
  IEventService eventService,
  ILogger<AdminEventController> logger) : ControllerBase
{
  [HttpPost("{id:guid}/reminder")]
  [EndpointSummary("Envoyer un rappel à tous les inscrits d'un événement")]
  public async Task<IActionResult> SendReminder(Guid id)
  {
    logger.LogInformation("POST /api/admin/events/{Id}/reminder", id);
    await eventService.SendReminderAsync(id);
    return NoContent();
  }
}
