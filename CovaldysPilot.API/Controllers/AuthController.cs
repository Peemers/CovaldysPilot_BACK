using System.Security.Claims;
using CovaldysPilot.Application.DTOs.Auth.Request;
using CovaldysPilot.Application.DTOs.Auth.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CovaldysPilot.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
  {
    #region Register
    /// <summary>
    /// Inscrit un nouveau membre sur la plateforme de manière asynchrone.
    /// </summary>
    /// <param name="dto">Le DTO contenant les informations nécessaires pour l'inscription.</param>
    /// <returns>Le DTO contenant les informations d'authentification et les jetons générés.</returns>
    /// <response code="201">Le membre a été créé et authentifié avec succès.</response>
    /// <response code="400">Les données fournies sont invalides.</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    [EndpointSummary("Inscription d'un nouveau membre")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto dto)
    {
      logger.LogInformation("Tentative d'inscription pour l'email : {Email}", dto.Email);
      AuthResponseDto result = await authService.RegisterAsync(dto);
      logger.LogInformation("Inscription réussie pour : {Pseudo}", dto.Pseudo);
      return CreatedAtAction(nameof(Register), result);
    }
    #endregion

    #region Login
    /// <summary>
    /// Authentifie un membre et génère les jetons d'accès de manière asynchrone.
    /// </summary>
    /// <param name="dto">Le DTO contenant les identifiants de connexion.</param>
    /// <returns>Le DTO contenant les informations d'authentification et les jetons générés.</returns>
    /// <response code="200">La connexion a été établie avec succès.</response>
    /// <response code="400">Les identifiants fournis sont incorrects ou invalides.</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    [EndpointSummary("Connexion d'un membre")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
      logger.LogInformation("Tentative de connexion pour : {EmailOrPseudo}", dto.EmailOrPseudo);
      AuthResponseDto result = await authService.LoginAsync(dto);
      logger.LogInformation("Connexion réussie pour : {EmailOrPseudo}", dto.EmailOrPseudo);
      return Ok(result);
    }
    #endregion

    #region Refresh
    /// <summary>
    /// Renouvelle le jeton d'accès (JWT) expiré à l'aide d'un jeton de rafraîchissement valide.
    /// </summary>
    /// <param name="dto">Le DTO contenant le jeton de rafraîchissement.</param>
    /// <returns>Le DTO contenant le nouveau jeton d'accès et le nouveau jeton de rafraîchissement.</returns>
    /// <response code="200">Les jetons ont été renouvelés avec succès.</response>
    /// <response code="400">Le jeton de rafraîchissement est invalide ou expiré.</response>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [EndpointSummary("Renouvellement du JWT")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
      logger.LogInformation("Tentative de refresh token");
      AuthResponseDto result = await authService.RefreshTokenAsync(dto);
      logger.LogInformation("Refresh token réussi");
      return Ok(result);
    }
    #endregion

    #region Logout
    /// <summary>
    /// Révoque le jeton de rafraîchissement d'un utilisateur pour le déconnecter.
    /// </summary>
    /// <param name="dto">Le DTO contenant le jeton de rafraîchissement à révoquer.</param>
    /// <returns>Un résultat vide confirmant la déconnexion.</returns>
    /// <response code="204">La déconnexion a été effectuée avec succès.</response>
    /// <response code="401">L'utilisateur n'est pas authentifié.</response>
    [HttpPost("logout")]
    [Authorize]
    [EndpointSummary("Déconnexion")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto)
    {
      logger.LogInformation("Déconnexion demandée");
      await authService.RevokeTokenAsync(dto.RefreshToken);
      logger.LogInformation("Déconnexion réussie");
      return NoContent();
    }
    #endregion

    #region ChangePassword
    /// <summary>
    /// Modifie le mot de passe de l'utilisateur actuellement connecté.
    /// </summary>
    /// <param name="dto">Le DTO contenant l'ancien et le nouveau mot de passe.</param>
    /// <returns>Un résultat vide confirmant la modification.</returns>
    /// <response code="204">Le mot de passe a été modifié avec succès.</response>
    /// <response code="401">L'utilisateur n'est pas authentifié.</response>
    [HttpPatch("change-password")]
    [Authorize]
    [EndpointSummary("Changer son mot de passe")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto dto)
    {
      // recuperation dans JWT
      string? userIdClaim = User.FindFirstValue("sub");
      if (!Guid.TryParse(userIdClaim, out Guid userId))
        return Unauthorized();
    
      logger.LogInformation("Changement de mot de passe pour : {UserId}", userId);
      await authService.ChangePasswordAsync(userId, dto);
      return NoContent();
    }
    #endregion
  }
}
