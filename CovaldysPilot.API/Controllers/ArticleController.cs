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

    private Guid? GetCurrentUserId()
    {
        string? userIdClaim = User.FindFirstValue("sub");
        return Guid.TryParse(userIdClaim, out Guid userId) ? userId : null;
    }
}