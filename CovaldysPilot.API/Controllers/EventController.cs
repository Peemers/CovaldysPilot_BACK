using System.Security.Claims;
using CovaldysPilot.Application.DTOs.Event.Request;
using CovaldysPilot.Application.DTOs.Event.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController(
  IEventService eventService,
  IBlobStorageService blobStorageService,
  ILogger<EventController> logger): ControllerBase
{
  #region GetAll
  /// <summary>
  /// Récupère tous les événements de manière asynchrone.
  /// </summary>
  /// <returns>Une collection de DTO de réponse contenant les informations des événements.</returns>
  /// <response code="200">La liste des événements a été récupérée avec succès.</response>
  [HttpGet]
  [AllowAnonymous]
  [EndpointSummary("Récupérer tous les événements")]
  [ProducesResponseType(typeof(IEnumerable<EventResponseDto>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<EventResponseDto>>> GetAll()
  {
    Guid? currentUserId = GetCurrentUserId();
    IEnumerable<EventResponseDto> events = await eventService.GetAllAsync(currentUserId);
    return Ok(events);
  }
  #endregion

  #region GetById
  /// <summary>
  /// Récupère un événement spécifique par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement.</param>
  /// <returns>Le DTO contenant les informations de l'événement s'il existe.</returns>
  /// <response code="200">L'événement a été récupéré avec succès.</response>
  /// <response code="404">L'événement demandé est introuvable.</response>
  [HttpGet("{id:guid}")]
  [AllowAnonymous]
  [EndpointSummary("Récupérer un événement par son ID")]
  [ProducesResponseType(typeof(EventResponseDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<EventResponseDto>> GetById(Guid id)
  {
    Guid? currentUserId = GetCurrentUserId();
    EventResponseDto? evt = await eventService.GetByIdAsync(id, currentUserId);
    if (evt == null) return NotFound();
    return Ok(evt);
  }
  #endregion

  #region Create
  /// <summary>
  /// Crée un nouvel événement de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les données de création de l'événement.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'événement créé.</returns>
  /// <response code="201">L'événement a été créé avec succès.</response>
  /// <response code="400">Les données de création fournies sont invalides.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPost]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Créer un événement")]
  [ProducesResponseType(typeof(EventResponseDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<EventResponseDto>> Create([FromBody] CreateEventRequestDto dto)
  {
    logger.LogInformation("POST /api/events - {Name}", dto.Name);
    EventResponseDto evt = await eventService.CreateAsync(dto);
    return CreatedAtAction(nameof(GetById), new { id = evt.Id }, evt);
  }
  #endregion

  #region Update
  /// <summary>
  /// Met à jour un événement existant de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement à modifier.</param>
  /// <param name="dto">Le DTO contenant les données de mise à jour de l'événement.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'événement mis à jour.</returns>
  /// <response code="200">L'événement a été mis à jour avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPut("{id:guid}")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Modifier un événement")]
  [ProducesResponseType(typeof(EventResponseDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<EventResponseDto>> Update(Guid id, [FromBody] UpdateEventRequestDto dto)
  {
    logger.LogInformation("PUT /api/events/{Id}", id);
    EventResponseDto evt = await eventService.UpdateAsync(id, dto);
    return Ok(evt);
  }
  #endregion

  #region Delete
  /// <summary>
  /// Supprime un événement par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement à supprimer.</param>
  /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
  /// <response code="204">L'événement a été supprimé avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpDelete("{id:guid}")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Supprimer un événement")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> Delete(Guid id)
  {
    logger.LogInformation("DELETE /api/events/{Id}", id);
    await eventService.DeleteAsync(id);
    return NoContent();
  }
  #endregion

  #region Cancel
  /// <summary>
  /// Annule un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement à annuler.</param>
  /// <param name="dto">Le DTO contenant le motif d'annulation.</param>
  /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
  /// <response code="204">L'événement a été annulé avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPatch("{id:guid}/cancel")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Annuler un événement")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelEventRequestDto dto)
  {
    logger.LogInformation("PATCH /api/events/{Id}/cancel", id);
    await eventService.CancelAsync(id, dto.CancellationReason);
    return NoContent();
  }
  #endregion

  #region Start
  /// <summary>
  /// Démarre un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement à démarrer.</param>
  /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
  /// <response code="204">L'événement a été démarré avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPatch("{id:guid}/start")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Démarrer un événement")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> Start(Guid id)
  {
    logger.LogInformation("PATCH /api/events/{Id}/start", id);
    await eventService.StartAsync(id);
    return NoContent();
  }
  #endregion

  #region Close
  /// <summary>
  /// Clôture un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement à clôturer.</param>
  /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
  /// <response code="204">L'événement a été clôturé avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPatch("{id:guid}/close")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Clôturer un événement")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> Close(Guid id)
  {
    logger.LogInformation("PATCH /api/events/{Id}/close", id);
    await eventService.CloseAsync(id);
    return NoContent();
  }
  #endregion
  
  #region GetStats
  /// <summary>
  /// Récupère les statistiques d'un événement par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement.</param>
  /// <returns>Le DTO contenant les statistiques de l'événement.</returns>
  /// <response code="200">Les statistiques de l'événement ont été récupérées avec succès.</response>
  [HttpGet("{id:guid}/stats")]
  [AllowAnonymous]
  [EndpointSummary("Récupérer les statistiques d'un événement")]
  [ProducesResponseType(typeof(EventStatsResponseDto), StatusCodes.Status200OK)]
  public async Task<ActionResult<EventStatsResponseDto>> GetStats(Guid id)
  {
    logger.LogInformation("GET /api/events/{Id}/stats", id);
    EventStatsResponseDto stats = await eventService.GetStatsAsync(id);
    return Ok(stats);
  }
  #endregion

  //methode privée, recuperation de l'id dans le token petite astuce reçue
  private Guid? GetCurrentUserId()
  {
    string? userIdClaim = User.FindFirstValue("sub");
    return Guid.TryParse(userIdClaim, out Guid userId) ? userId : null;
  }
  
  #region UploadCoverImage
  /// <summary>
  /// Téléverse une image de couverture pour un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement.</param>
  /// <param name="file">Le fichier image de couverture à téléverser.</param>
  /// <returns>Un objet anonyme contenant l'URL de l'image téléversée.</returns>
  /// <response code="200">L'image de couverture a été mise à jour avec succès.</response>
  /// <response code="400">Le fichier fourni est manquant, dans un format non supporté, ou dépasse la taille maximale autorisée.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPost("{id:guid}/upload-image")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Upload une image de couverture")]
  [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
  #endregion
}