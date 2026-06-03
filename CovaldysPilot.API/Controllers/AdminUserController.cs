using CovaldysPilot.Application.DTOs.User.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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
  [HttpGet]
  [EndpointSummary("Récupérer tous les membres")]
  public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
  {
    logger.LogInformation("GET /api/admin/users");
    IEnumerable<UserResponseDto> users = await userService.GetAllAsync();
    return Ok(users);
  }

  [HttpGet("{id:guid}")]
  [EndpointSummary("Récupérer un membre par son ID")]
  public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
  {
    logger.LogInformation("GET /api/admin/users/{Id}", id);
    UserResponseDto? user = await userService.GetByIdAsync(id);
    if (user is null) return NotFound();
    return Ok(user);
  }

  [HttpDelete("{id:guid}")]
  [EndpointSummary("Supprimer un membre")]
  public async Task<IActionResult> Delete(Guid id)
  {
    logger.LogInformation("DELETE /api/admin/users/{Id}", id);
    await userService.DeleteAsync(id);
    return NoContent();
  }
}