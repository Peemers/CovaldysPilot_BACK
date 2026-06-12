using System.Security.Claims;
using CovaldysPilot.Application.DTOs.User.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController(
  IUserService userService,
  ILogger<UserController> logger) : ControllerBase
{
  [HttpGet("me")]
  [EndpointSummary("Récupérer son propre profil")]
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
}