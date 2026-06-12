using System.Security.Claims;
using CovaldysPilot.Application.DTOs.Article.Request;
using CovaldysPilot.Application.DTOs.Article.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController(
    IArticleService articleService,
    IBlobStorageService blobStorageService,
    ILogger<ArticleController> logger) : ControllerBase
{
    #region GetAll
    /// <summary>
    /// Récupère la liste de tous les articles de manière asynchrone.
    /// </summary>
    /// <returns>Une collection de DTO contenant les informations des articles.</returns>
    /// <response code="200">La liste des articles a été récupérée avec succès.</response>
    [HttpGet]
    [AllowAnonymous]
    [EndpointSummary("Récupérer tous les articles")]
    [ProducesResponseType(typeof(IEnumerable<ArticleResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ArticleResponseDto>>> GetAll()
    {
        logger.LogInformation("GET /api/articles");
        IEnumerable<ArticleResponseDto> articles = await articleService.GetAllAsync();
        return Ok(articles);
    }
    #endregion

    #region GetById
    /// <summary>
    /// Récupère un article spécifique par son identifiant unique de manière asynchrone.
    /// </summary>
    /// <param name="id">L'identifiant unique de l'article.</param>
    /// <returns>Le DTO contenant les informations de l'article s'il existe.</returns>
    /// <response code="200">L'article a été récupéré avec succès.</response>
    /// <response code="404">L'article demandé est introuvable.</response>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [EndpointSummary("Récupérer un article par son ID")]
    [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ArticleResponseDto>> GetById(Guid id)
    {
        logger.LogInformation("GET /api/articles/{Id}", id);
        ArticleResponseDto? article = await articleService.GetByIdAsync(id);
        if (article is null) return NotFound();
        return Ok(article);
    }
    #endregion

    #region Create
    /// <summary>
    /// Crée un nouvel article de manière asynchrone.
    /// </summary>
    /// <param name="dto">Le DTO contenant les données de création de l'article.</param>
    /// <returns>Le DTO de réponse contenant les détails de l'article créé.</returns>
    /// <response code="201">L'article a été créé avec succès.</response>
    /// <response code="401">L'utilisateur n'est pas authentifié.</response>
    /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Créer un article")]
    [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ArticleResponseDto>> Create([FromBody] CreateArticleRequestDto dto)
    {
        Guid? userId = GetCurrentUserId();
        logger.LogInformation("POST /api/articles - {Title}", dto.Title);
        ArticleResponseDto article = await articleService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = article.Id }, article);
    }
    #endregion

    #region Update
    /// <summary>
    /// Met à jour un article existant de manière asynchrone.
    /// </summary>
    /// <param name="id">L'identifiant unique de l'article à modifier.</param>
    /// <param name="dto">Le DTO contenant les nouvelles données de l'article.</param>
    /// <returns>Le DTO de réponse contenant les détails de l'article mis à jour.</returns>
    /// <response code="200">L'article a été modifié avec succès.</response>
    /// <response code="401">L'utilisateur n'est pas authentifié.</response>
    /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Modifier un article")]
    [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ArticleResponseDto>> Update(Guid id, [FromBody] UpdateArticleRequestDto dto)
    {
        logger.LogInformation("PUT /api/articles/{Id}", id);
        ArticleResponseDto article = await articleService.UpdateAsync(id, dto);
        return Ok(article);
    }
    #endregion

    #region Delete
    /// <summary>
    /// Supprime un article par son identifiant unique de manière asynchrone.
    /// </summary>
    /// <param name="id">L'identifiant unique de l'article à supprimer.</param>
    /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
    /// <response code="204">L'article a été supprimé avec succès.</response>
    /// <response code="401">L'utilisateur n'est pas authentifié.</response>
    /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer un article")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete(Guid id)
    {
        logger.LogInformation("DELETE /api/articles/{Id}", id);
        await articleService.DeleteAsync(id);
        return NoContent();
    }
    #endregion

    #region UploadImage
    /// <summary>
    /// Ajoute une image à un article existant de manière asynchrone.
    /// </summary>
    /// <param name="id">L'identifiant unique de l'article.</param>
    /// <param name="file">Le fichier image à téléverser.</param>
    /// <returns>Le DTO contenant les informations de l'article avec sa nouvelle image.</returns>
    /// <response code="200">L'image a été ajoutée et associée à l'article avec succès.</response>
    /// <response code="400">Le fichier fourni est manquant, dans un format non supporté, ou dépasse la taille maximale autorisée.</response>
    /// <response code="401">L'utilisateur n'est pas authentifié.</response>
    /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
    [HttpPost("{id:guid}/upload-image")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Ajouter une image à un article")]
    [ProducesResponseType(typeof(ArticleResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ArticleResponseDto>> UploadImage(Guid id, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Aucun fichier fourni.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest("Format non supporté. Utilisez JPG, PNG ou WebP.");

        if (file.Length > 10 * 1024 * 1024)
            return BadRequest("L'image ne doit pas dépasser 10MB.");

        logger.LogInformation("POST /api/articles/{Id}/upload-image", id);

        await using var stream = file.OpenReadStream();
        var url = await blobStorageService.UploadAsync(stream, file.FileName, file.ContentType);
        ArticleResponseDto article = await articleService.AddImageAsync(id, url);
        return Ok(article);
    }
    #endregion

    #region DeleteImage
    /// <summary>
    /// Supprime une image associée à un article de manière asynchrone.
    /// </summary>
    /// <param name="id">L'identifiant unique de l'article.</param>
    /// <param name="imageId">L'identifiant unique de l'image à supprimer.</param>
    /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
    /// <response code="204">L'image a été supprimée de l'article avec succès.</response>
    /// <response code="401">L'utilisateur n'est pas authentifié.</response>
    /// <response code="403">L'utilisateur n'est pas autorisé à effectuer cette action (rôle Admin requis).</response>
    [HttpDelete("{id:guid}/images/{imageId:guid}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer une image d'un article")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteImage(Guid id, Guid imageId)
    {
        logger.LogInformation("DELETE /api/articles/{Id}/images/{ImageId}", id, imageId);
        await articleService.DeleteImageAsync(id, imageId);
        return NoContent();
    }
    #endregion

    private Guid? GetCurrentUserId()
    {
        string? userIdClaim = User.FindFirstValue("sub");
        return Guid.TryParse(userIdClaim, out Guid userId) ? userId : null;
    }
}