using System.Security.Claims;
using CovaldysPilot.Application.DTOs.Article.Request;
using CovaldysPilot.Application.DTOs.Article.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController(
    IArticleService articleService,
    IBlobStorageService blobStorageService,
    ILogger<ArticleController> logger) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [EndpointSummary("Récupérer tous les articles")]
    public async Task<ActionResult<IEnumerable<ArticleResponseDto>>> GetAll()
    {
        logger.LogInformation("GET /api/articles");
        IEnumerable<ArticleResponseDto> articles = await articleService.GetAllAsync();
        return Ok(articles);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [EndpointSummary("Récupérer un article par son ID")]
    public async Task<ActionResult<ArticleResponseDto>> GetById(Guid id)
    {
        logger.LogInformation("GET /api/articles/{Id}", id);
        ArticleResponseDto? article = await articleService.GetByIdAsync(id);
        if (article is null) return NotFound();
        return Ok(article);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Créer un article")]
    public async Task<ActionResult<ArticleResponseDto>> Create([FromBody] CreateArticleRequestDto dto)
    {
        Guid? userId = GetCurrentUserId();
        logger.LogInformation("POST /api/articles - {Title}", dto.Title);
        ArticleResponseDto article = await articleService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = article.Id }, article);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Modifier un article")]
    public async Task<ActionResult<ArticleResponseDto>> Update(Guid id, [FromBody] UpdateArticleRequestDto dto)
    {
        logger.LogInformation("PUT /api/articles/{Id}", id);
        ArticleResponseDto article = await articleService.UpdateAsync(id, dto);
        return Ok(article);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer un article")]
    public async Task<IActionResult> Delete(Guid id)
    {
        logger.LogInformation("DELETE /api/articles/{Id}", id);
        await articleService.DeleteAsync(id);
        return NoContent();
    }
    
    [HttpPost("{id:guid}/upload-image")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Ajouter une image à un article")]
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

    [HttpDelete("{id:guid}/images/{imageId:guid}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer une image d'un article")]
    public async Task<IActionResult> DeleteImage(Guid id, Guid imageId)
    {
        logger.LogInformation("DELETE /api/articles/{Id}/images/{ImageId}", id, imageId);
        await articleService.DeleteImageAsync(id, imageId);
        return NoContent();
    }

    private Guid? GetCurrentUserId()
    {
        string? userIdClaim = User.FindFirstValue("sub");
        return Guid.TryParse(userIdClaim, out Guid userId) ? userId : null;
    }
}