using System.Security.Claims;
using CovaldysPilot.Application.DTOs.Event.Request;
using CovaldysPilot.Application.DTOs.Event.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController(
  IEventService eventService,
  IBlobStorageService blobStorageService,
  ILogger<EventController> logger): ControllerBase
{
  [HttpGet]
  [AllowAnonymous]
  [EndpointSummary("Récupérer tous les événements")]
  public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetAll()
  {
    Guid? currentUserId = GetCurrentUserId();
    IEnumerable<EventResponseDto> events = await eventService.GetAllAsync(currentUserId);
    return Ok(events);
  }

  [HttpGet("{id:guid}")]
  [AllowAnonymous]
  [EndpointSummary("Récupérer un événement par son ID")]
  public async Task<ActionResult<EventResponseDto>> GetById(Guid id)
  {
    Guid? currentUserId = GetCurrentUserId();
    EventResponseDto? evt = await eventService.GetByIdAsync(id, currentUserId);
    if (evt == null) return NotFound();
    return Ok(evt);
  }

  [HttpPost]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Créer un événement")]
  public async Task<ActionResult<EventResponseDto>> Create([FromBody] CreateEventRequestDto dto)
  {
    logger.LogInformation("POST /api/events - {Name}", dto.Name);
    EventResponseDto evt = await eventService.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { id = evt.Id }, evt);
  }

  [HttpPut("{id:guid}")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Modifier un événement")]
  public async Task<ActionResult<EventResponseDto>> Update(Guid id, [FromBody] UpdateEventRequestDto dto)
  {
    logger.LogInformation("PUT /api/events/{Id}", id);
    EventResponseDto evt = await eventService.UpdateAsync(id, dto);
    return Ok(evt);
  }

  [HttpDelete("{id:guid}")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Supprimer un événement")]
  public async Task<IActionResult> Delete(Guid id)
  {
    logger.LogInformation("DELETE /api/events/{Id}", id);
    await eventService.DeleteAsync(id);
    return NoContent();
  }

  [HttpPatch("{id:guid}/cancel")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Annuler un événement")]
  public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelEventRequestDto dto)
  {
    logger.LogInformation("PATCH /api/events/{Id}/cancel", id);
    await eventService.CancelAsync(id, dto.CancellationReason);
    return NoContent();
  }

  [HttpPatch("{id:guid}/start")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Démarrer un événement")]
  public async Task<IActionResult> Start(Guid id)
  {
    logger.LogInformation("PATCH /api/events/{Id}/start", id);
    await eventService.StartAsync(id);
    return NoContent();
  }

  [HttpPatch("{id:guid}/close")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Clôturer un événement")]
  public async Task<IActionResult> Close(Guid id)
  {
    logger.LogInformation("PATCH /api/events/{Id}/close", id);
    await eventService.CloseAsync(id);
    return NoContent();
  }
  
  [HttpGet("{id:guid}/stats")]
  [AllowAnonymous]
  [EndpointSummary("Récupérer les statistiques d'un événement")]
  public async Task<ActionResult<EventStatsResponseDto>> GetStats(Guid id)
  {
    logger.LogInformation("GET /api/events/{Id}/stats", id);
    EventStatsResponseDto stats = await eventService.GetStatsAsync(id);
    return Ok(stats);
  }

  //methode privée, recuperation de l'id dans le token petite astuce reçue
  private Guid? GetCurrentUserId()
  {
    string? userIdClaim = User.FindFirstValue("sub");
    return Guid.TryParse(userIdClaim, out Guid userId) ? userId : null;
  }
  
  [HttpPost("{id:guid}/upload-image")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Upload une image de couverture")]
  public async Task<ActionResult<string>> UploadCoverImage(Guid id, IFormFile file)
  {
    if (file == null || file.Length == 0)
      return BadRequest("Aucun fichier fourni.");

    // Verif si bien une image
    var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
    if (!allowedTypes.Contains(file.ContentType))
      return BadRequest("Format non supporté. Utilisez JPG, PNG ou WebP.");

    // Verif la taille max 10MB
    if (file.Length > 10 * 1024 * 1024)
      return BadRequest("L'image ne doit pas dépasser 5MB.");

    logger.LogInformation("POST /api/events/{Id}/upload-image", id);

    await using var stream = file.OpenReadStream();
    var url = await blobStorageService.UploadAsync(stream, file.FileName, file.ContentType);

    // Mettre a jour l'event avec la nouvelle URL
    await eventService.UpdateCoverImageAsync(id, url);

    return Ok(new { url });
  }
}