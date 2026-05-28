using CovaldysPilot.Application.DTOs.Category.Request;
using CovaldysPilot.Application.DTOs.Category.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(
  ICategoryService categoryService,
  ILogger<CategoryController> logger) : ControllerBase
{
  [HttpGet]
  [AllowAnonymous]
  [EndpointSummary("Récupérer toutes les catégories")]
  public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
  {
    logger.LogInformation("GET /api/categories");
    IEnumerable<CategoryResponseDto> categories = await categoryService.GetAllAsync();
    return Ok(categories);
  }

  [HttpPost]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Créer une catégorie")]
  public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CreateCategoryRequestDto dto)
  {
    logger.LogInformation("POST /api/categories - {Name}", dto.Name);
    CategoryResponseDto category = await categoryService.CreateAsync(dto);
    return CreatedAtAction(nameof(GetAll), category);
  }

  [HttpDelete("{id:guid}")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Supprimer une catégorie")]
  public async Task<IActionResult> Delete(Guid id)
  {
    logger.LogInformation("DELETE /api/categories/{Id}", id);
    await categoryService.DeleteAsync(id);
    return NoContent();
  }
}