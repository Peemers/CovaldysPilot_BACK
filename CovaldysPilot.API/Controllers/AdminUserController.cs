using CovaldysPilot.Application.DTOs.User.Request;
using CovaldysPilot.Application.DTOs.User.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

//controller pour admin séparé pour éviter les erreurs de Authorize

[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUserController(
  IUserService userService,
  ILogger<AdminUserController> logger) : ControllerBase
{
  #region GetAll
  /// <summary>
  /// Récupère tous les membres inscrits sur la plateforme de manière asynchrone.
  /// </summary>
  /// <returns>Une collection de DTO contenant les informations des membres.</returns>
  /// <response code="200">La liste des membres a été récupérée avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpGet]
  [EndpointSummary("Récupérer tous les membres")]
  [ProducesResponseType(typeof(IEnumerable<UserResponseDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
  {
    logger.LogInformation("GET /api/admin/users");
    IEnumerable<UserResponseDto> users = await userService.GetAllAsync();
    return Ok(users);
  }
  #endregion

  #region GetById
  /// <summary>
  /// Récupère un membre spécifique par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique du membre.</param>
  /// <returns>Le DTO contenant les informations du membre s'il existe.</returns>
  /// <response code="200">Le membre a été récupéré avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  /// <response code="404">Le membre demandé est introuvable.</response>
  [HttpGet("{id:guid}")]
  [EndpointSummary("Récupérer un membre par son ID")]
  [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
  {
    logger.LogInformation("GET /api/admin/users/{Id}", id);
    UserResponseDto? user = await userService.GetByIdAsync(id);
    if (user is null) return NotFound();
    return Ok(user);
  }
  #endregion

  #region Delete
  /// <summary>
  /// Supprime un membre spécifique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique du membre à supprimer.</param>
  /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
  /// <response code="204">Le membre a été supprimé avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpDelete("{id:guid}")]
  [EndpointSummary("Supprimer un membre")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> Delete(Guid id)
  {
    logger.LogInformation("DELETE /api/admin/users/{Id}", id);
    await userService.DeleteAsync(id);
    return NoContent();
  }
  #endregion

  #region Export
  /// <summary>
  /// Exporte la liste des membres vers un fichier Excel de manière asynchrone.
  /// </summary>
  /// <param name="filter">Le filtre optionnel à appliquer sur la catégorie des membres.</param>
  /// <returns>Un fichier Excel (.xlsx) contenant la liste des membres filtrée.</returns>
  /// <response code="200">Le fichier Excel a été généré et téléchargé avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpGet("export")]
  [EndpointSummary("Exporter la liste des membres en Excel")]
  [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> Export([FromQuery] string? filter = null)
  {
    logger.LogInformation("GET /api/admin/users/export - Filtre: {Filter}", filter ?? "all");
    byte[] fileBytes = await userService.ExportMembersAsync(filter);
    return File(
      fileBytes, "application/vnd.openxmlformats-officedocument" +
                 ".spreadsheetml.sheet", $"membres_{filter ?? "all"}_{DateTime.UtcNow:yyyyMMdd}.xlsx");
  }
  #endregion

  #region AddManually
  /// <summary>
  /// Ajoute manuellement un nouveau membre de manière asynchrone (sans inscription publique).
  /// </summary>
  /// <param name="dto">Le DTO contenant les informations du membre à créer.</param>
  /// <returns>Le DTO de réponse contenant les informations du membre créé.</returns>
  /// <response code="201">Le membre a été créé manuellement avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPost]
  [EndpointSummary("Ajouter un membre manuellement")]
  [ProducesResponseType(typeof(CreateUserManuallyResponseDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<CreateUserManuallyResponseDto>> AddManually([FromBody] CreateUserManuallyRequestDto dto)
  {
    logger.LogInformation("POST /api/admin/users - Ajout manuel de {Email}", dto.Email);
    CreateUserManuallyResponseDto user = await userService.AddManuallyAsync(dto);
    return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
  }
  #endregion
}