using CovaldysPilot.Application.DTOs.Category.Request;
using CovaldysPilot.Application.DTOs.Category.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(
  ICategoryService categoryService,
  ILogger<CategoryController> logger) : ControllerBase
{
  #region GetAll
  /// <summary>
  /// Récupère la liste de toutes les catégories de manière asynchrone.
  /// </summary>
  /// <returns>Une collection de DTO contenant les informations des catégories.</returns>
  /// <response code="200">La liste des catégories a été récupérée avec succès.</response>
  [HttpGet]
  [AllowAnonymous]
  [EndpointSummary("Récupérer toutes les catégories")]
  [ProducesResponseType(typeof(IEnumerable<CategoryResponseDto>), StatusCodes.Status200OK)]
  public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
  {
    logger.LogInformation("GET /api/categories");
    IEnumerable<CategoryResponseDto> categories = await categoryService.GetAllAsync();
    return Ok(categories);
  }
  #endregion

  #region Create
  /// <summary>
  /// Crée une nouvelle catégorie de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les données de création de la catégorie.</param>
  /// <returns>Le DTO de réponse contenant les détails de la catégorie créée.</returns>
  /// <response code="201">La catégorie a été créée avec succès.</response>
  /// <response code="400">Les données de création fournies sont invalides.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpPost]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Créer une catégorie")]
  [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CreateCategoryRequestDto dto)
  {
    logger.LogInformation("POST /api/categories - {Name}", dto.Name);
    CategoryResponseDto category = await categoryService.CreateAsync(dto);
    return CreatedAtAction(nameof(GetAll), category);
  }
  #endregion

  #region Delete
  /// <summary>
  /// Supprime une catégorie par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de la catégorie à supprimer.</param>
  /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
  /// <response code="204">La catégorie a été supprimée avec succès.</response>
  /// <response code="401">L'utilisateur n'est pas authentifié.</response>
  /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
  [HttpDelete("{id:guid}")]
  [Authorize(Roles = "Admin")]
  [EndpointSummary("Supprimer une catégorie")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<IActionResult> Delete(Guid id)
  {
    logger.LogInformation("DELETE /api/categories/{Id}", id);
    await categoryService.DeleteAsync(id);
    return NoContent();
  }
  #endregion
}