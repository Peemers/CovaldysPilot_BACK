using System.Security.Claims;
using CovaldysPilot.Application.DTOs.User.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController(
  IUserService userService,
  ILogger<UserController> logger) : ControllerBase
{
  #region GetMe
  /// <summary>
  /// Récupère le profil du membre actuellement connecté de manière asynchrone.
  /// </summary>
  /// <returns>Le DTO contenant les informations du membre connecté.</returns>
  /// <response code="200">Le profil de l'utilisateur a été récupéré avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="404">Le membre est introuvable.</response>
  [HttpGet("me")]
  [EndpointSummary("Récupérer son propre profil")]
  [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<UserResponseDto>> GetMe()
  {
    string? userIdClaim = User.FindFirstValue("sub");
    if (!Guid.TryParse(userIdClaim, out Guid userId))
      return Unauthorized();

    logger.LogInformation("GET /api/users/me - UserId: {UserId}", userId);
    UserResponseDto? user = await userService.GetByIdAsync(userId);
    if (user is null) return NotFound();
    return Ok(user);
  }
  #endregion
  
  #region PayCotisation
  /// <summary>
  /// Simule le paiement de la cotisation annuelle de 10€ pour le membre connecté.
  /// </summary>
  /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
  /// <response code="204">La cotisation a été payée avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  [HttpPatch("me/cotisation")]
  [EndpointSummary("Simuler le paiement de la cotisation annuelle")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> PayCotisation()
  {
    string? userIdClaim = User.FindFirstValue("sub");
    if (!Guid.TryParse(userIdClaim, out Guid userId))
      return Unauthorized();

    logger.LogInformation("PATCH /api/users/me/cotisation - UserId: {UserId}", userId);
    await userService.PayCotisationAsync(userId);
    return NoContent();
  }
  #endregion
}