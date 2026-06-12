using System.Security.Claims;
using CovaldysPilot.Application.DTOs.SignIn.Request;
using CovaldysPilot.Application.DTOs.SignIn.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SignInController(
  ISignInService signInService,
  ILogger<SignInController> logger) : ControllerBase
{
  #region GetByUser
  /// <summary>
  /// Récupère la liste des inscriptions du membre actuellement connecté de manière asynchrone.
  /// </summary>
  /// <returns>Une collection de DTO de réponse contenant les informations des inscriptions.</returns>
  /// <response code="200">La liste des inscriptions a été récupérée avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  [HttpGet("user")]
  [EndpointSummary("Récupérer mes inscriptions")]
  [ProducesResponseType(typeof(IEnumerable<SignInResponseDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<IEnumerable<SignInResponseDto>>> GetByUser()
  {
    Guid userId = GetCurrentUserId();
    IEnumerable<SignInResponseDto> signIns = await signInService.GetByUserAsync(userId);
    return Ok(signIns);
  }
  #endregion

  #region Register
  /// <summary>
  /// Inscrit le membre connecté à un événement de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les données de l'inscription.</param>
  /// <returns>Le DTO contenant les détails de l'inscription créée.</returns>
  /// <response code="201">L'inscription a été créée avec succès.</response>
  /// <response code="400">Les données fournies sont invalides.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  [HttpPost]
  [EndpointSummary("S'inscrire à un événement")]
  [ProducesResponseType(typeof(SignInResponseDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<SignInResponseDto>> Register([FromBody] CreateSignInRequestDto dto)
  {
    Guid userId = GetCurrentUserId();
    logger.LogInformation("POST /api/signin - UserId: {UserId}", userId);
    SignInResponseDto signIn = await signInService.RegisterAsync(userId, dto);
    return CreatedAtAction(nameof(GetByUser), signIn);
  }
  #endregion

  #region Unregister
  /// <summary>
  /// Désinscrit le membre connecté d'un événement de manière asynchrone.
  /// </summary>
  /// <param name="signInId">L'identifiant unique de l'inscription à annuler.</param>
  /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
  /// <response code="204">La désinscription a été effectuée avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  [HttpDelete("{signInId:guid}")]
  [EndpointSummary("Se désinscrire d'un événement")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Unregister(Guid signInId)
  {
    Guid userId = GetCurrentUserId();
    logger.LogInformation("Delete /api/signin - UserId: {UserId}", userId);
    await signInService.UnregisterAsync(userId, signInId);
    return NoContent();
  }
  #endregion

  #region GetByEvents
  /// <summary>
  /// Récupère toutes les inscriptions pour un événement spécifique de manière asynchrone.
  /// </summary>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>Une collection de DTO contenant les inscriptions associées à l'événement.</returns>
  /// <response code="200">La liste des inscriptions a été récupérée avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpGet("event/{eventId:guid}")]
  [EndpointSummary("Recuperer les inscriptions d'un événement")]
  [Authorize(Roles = "Admin")]
  [ProducesResponseType(typeof(IEnumerable<SignInResponseDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<IEnumerable<SignInResponseDto>>> GetByEvents(Guid eventId)
  {
    IEnumerable<SignInResponseDto> signIns = await signInService.GetByEventAsync(eventId);
    return Ok(signIns);
  }
  #endregion

  #region Private Methode : GetCurrentUserId
  private Guid GetCurrentUserId()
  {
    string? userIdClaim = User.FindFirstValue("sub");
    if (!Guid.TryParse(userIdClaim, out Guid userId))
      throw new UnauthorizedAccessException("Utilisateur non authentifié.");
    return userId;
  }
  #endregion

  #region ValidatePayment
  /// <summary>
  /// Valide le paiement d'une inscription spécifique de manière asynchrone.
  /// </summary>
  /// <param name="signInId">L'identifiant unique de l'inscription dont le paiement doit être validé.</param>
  /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
  /// <response code="204">Le paiement a été validé avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPatch("{signInId:guid}/validate")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Valider le paiement d'une inscription")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> ValidatePayment(Guid signInId)
  {
    logger.LogInformation("PATCH /api/signin/{SignInId}/validate", signInId);
    await signInService.ValidatePayment(signInId);
    return NoContent();
  }
  #endregion
}