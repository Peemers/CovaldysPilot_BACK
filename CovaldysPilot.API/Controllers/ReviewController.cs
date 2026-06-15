using System.Security.Claims;
using CovaldysPilot.Application.DTOs.Review.Request;
using CovaldysPilot.Application.DTOs.Review.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController(
    IReviewService reviewService,
    ILogger<ReviewController> logger) : ControllerBase
{
    #region GetByEvent
    /// <summary>
    /// Récupère la liste des avis associés à un événement spécifique de manière asynchrone.
    /// </summary>
    /// <param name="eventId">L'identifiant unique de l'événement.</param>
    /// <returns>Une collection de DTO contenant les informations des avis.</returns>
    /// <response code="200">La liste des avis a été récupérée avec succès.</response>
    [HttpGet("event/{eventId:guid}")]
    [AllowAnonymous]
    [EndpointSummary("Récupérer les avis d'un événement")]
    [ProducesResponseType(typeof(IEnumerable<ReviewResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetByEvent(Guid eventId)
    {
        logger.LogInformation("GET /api/reviews/event/{EventId}", eventId);
        IEnumerable<ReviewResponseDto> reviews = await reviewService.GetByEventAsync(eventId);
        return Ok(reviews);
    }
    #endregion

    #region Create
    /// <summary>
    /// Crée un nouvel avis pour un événement terminé de manière asynchrone.
    /// </summary>
    /// <param name="dto">Le DTO contenant les données de l'avis à créer.</param>
    /// <returns>Le DTO de réponse contenant les détails de l'avis créé.</returns>
    /// <response code="201">L'avis a été créé avec succès.</response>
    /// <response code="400">Les données fournies sont invalides.</response>
    /// <response code="401">L'utilisateur n'est pas authentifié.</response>
    [HttpPost]
    [Authorize]
    [EndpointSummary("Laisser un avis sur un événement terminé")]
    [ProducesResponseType(typeof(ReviewResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ReviewResponseDto>> Create([FromBody] CreateReviewRequestDto dto)
    {
        Guid? userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();

        logger.LogInformation("POST /api/reviews - UserId: {UserId}", userId);
        ReviewResponseDto review = await reviewService.CreateAsync(userId.Value, dto);
        return CreatedAtAction(nameof(GetByEvent), new { eventId = review.EventId }, review);
    }
    #endregion

    #region Update
    /// <summary>
    /// Met à jour un avis existant rédigé par l'utilisateur connecté de manière asynchrone.
    /// </summary>
    /// <param name="id">L'identifiant unique de l'avis à modifier.</param>
    /// <param name="dto">Le DTO contenant les nouvelles données de l'avis.</param>
    /// <returns>Le DTO de réponse contenant les détails de l'avis mis à jour.</returns>
    /// <response code="200">L'avis a été mis à jour avec succès.</response>
    /// <response code="400">Les données fournies sont invalides.</response>
    /// <response code="401">L'utilisateur n'est pas authentifié.</response>
    [HttpPut("{id:guid}")]
    [Authorize]
    [EndpointSummary("Modifier son avis")]
    [ProducesResponseType(typeof(ReviewResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ReviewResponseDto>> Update(Guid id, [FromBody] UpdateReviewRequestDto dto)
    {
        Guid? userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();

        logger.LogInformation("PUT /api/reviews/{Id}", id);
        ReviewResponseDto review = await reviewService.UpdateAsync(userId.Value, id, dto);
        return Ok(review);
    }
    #endregion

    #region Delete
    /// <summary>
    /// Supprime un avis existant rédigé par l'utilisateur connecté de manière asynchrone.
    /// </summary>
    /// <param name="id">L'identifiant unique de l'avis à supprimer.</param>
    /// <returns>Un résultat vide indiquant la réussite de l'opération.</returns>
    /// <response code="204">L'avis a été supprimé avec succès.</response>
    /// <response code="401">L'utilisateur n'est pas authentifié.</response>
    [HttpDelete("{id:guid}")]
    [Authorize]
    [EndpointSummary("Supprimer son avis")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id)
    {
        Guid? userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();

        logger.LogInformation("DELETE /api/reviews/{Id}", id);
        await reviewService.DeleteAsync(userId.Value, id);
        return NoContent();
    }
    #endregion

    private Guid? GetCurrentUserId()
    {
        string? userIdClaim = User.FindFirstValue("sub");
        return Guid.TryParse(userIdClaim, out Guid userId) ? userId : null;
    }
}