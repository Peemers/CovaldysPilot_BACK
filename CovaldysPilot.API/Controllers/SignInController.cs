using System.Security.Claims;
using CovaldysPilot.Application.DTOs.SignIn.Request;
using CovaldysPilot.Application.DTOs.SignIn.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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

  [HttpGet("user")]
  [EndpointSummary("Récupérer mes inscriptions")]
  public async Task<ActionResult<IEnumerable<SignInResponseDto>>> GetByUser()
  {
    Guid userId = GetCurrentUserId();
    IEnumerable<SignInResponseDto> signIns = await signInService.GetByUserAsync(userId);
    return Ok(signIns);
  }

  #endregion

  #region Register-SignIn

  [HttpPost]
  [EndpointSummary("S'inscrire à un événement")]
  public async Task<ActionResult<SignInResponseDto>> Register([FromBody] CreateSignInRequestDto dto)
  {
    Guid userId = GetCurrentUserId();
    logger.LogInformation("POST /api/signin - UserId: {UserId}", userId);
    SignInResponseDto signIn = await signInService.RegisterAsync(userId, dto);
    return CreatedAtAction(nameof(GetByUser), signIn);
  }

  #endregion

  #region Unregister-SignIn

  [HttpDelete("{SignInId:guid}")]
  [EndpointSummary("Se désinscrire d'un événement")]
  public async Task<IActionResult> Unregister(Guid signInId)
  {
    Guid userId = GetCurrentUserId();
    logger.LogInformation("Delete /api/signin - UserId: {UserId}", userId);
    await signInService.UnregisterAsync(userId, signInId);
    return NoContent();
  }

  #endregion

  #region GetByEvent

  [HttpGet("event/{eventId:guid}")]
  [EndpointSummary("Recuperer les inscriptions d'un événement")]
  [Authorize(Roles = "Admin")]
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

  [HttpPatch("{signInId:guid}/validate")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Valider le paiement d'une inscription")]
  public async Task<IActionResult> ValidatePayment(Guid signInId)
  {
    logger.LogInformation("PATCH /api/signin/{SignInId}/validate", signInId);
    await signInService.ValidatePayment(signInId);
    return NoContent();
  }

  #endregion
}