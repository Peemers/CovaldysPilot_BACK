using CovaldysPilot.Application.DTOs.Auth.Request;
using CovaldysPilot.Application.DTOs.Auth.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CovaldysPilot.API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
  {
    [HttpPost("register")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    [EndpointSummary("Inscription d'un nouveau membre")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto dto)
    {
      logger.LogInformation("Tentative d'inscription pour l'email : {Email}", dto.Email);
      AuthResponseDto result = await authService.RegisterAsync(dto);
      logger.LogInformation("Inscription réussie pour : {Pseudo}", dto.Pseudo);
      return CreatedAtAction(nameof(Register), result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    [EndpointSummary("Connexion d'un membre")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
      logger.LogInformation("Tentative de connexion pour : {EmailOrPseudo}", dto.EmailOrPseudo);
      AuthResponseDto result = await authService.LoginAsync(dto);
      logger.LogInformation("Connexion réussie pour : {EmailOrPseudo}", dto.EmailOrPseudo);
      return Ok(result);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [EndpointSummary("Renouvellement du JWT")]
    public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshTokenRequestDto dto)
    {
      logger.LogInformation("Tentative de refresh token");
      AuthResponseDto result = await authService.RefreshTokenAsync(dto);
      logger.LogInformation("Refresh token réussi");
      return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    [EndpointSummary("Déconnexion")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto)
    {
      logger.LogInformation("Déconnexion demandée");
      await authService.RevokeTokenAsync(dto.RefreshToken);
      logger.LogInformation("Déconnexion réussie");
      return NoContent();
    }
  }
}