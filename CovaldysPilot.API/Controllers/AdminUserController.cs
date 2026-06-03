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

  [HttpGet("export")]
  [EndpointSummary("Exporter la liste des membres en Excel")]
  // pas oublié filter si je veux une autre catégorie (membre effectif etc), fromquery car c# doit regarder l'url pour le para
  public async Task<IActionResult> Export([FromQuery] string? filter = null)
  {
    logger.LogInformation("GET /api/admin/users/export - Filtre: {Filter}", filter ?? "all");
    byte[] fileBytes = await userService.ExportMembersAsync(filter);
    //MIME officiel pour excel sans ça le navigateur ne sais rien faire du .xlsx
    return File(
      fileBytes, "application/vnd.openxmlformats-officedocument" +
                 ".spreadsheetml.sheet", $"membres_{filter ?? "all"}_{DateTime.UtcNow:yyyyMMdd}.xlsx"); //nom du fichier
  }
}