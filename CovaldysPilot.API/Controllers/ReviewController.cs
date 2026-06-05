using System.Security.Claims;
using CovaldysPilot.Application.DTOs.Review.Request;
using CovaldysPilot.Application.DTOs.Review.Response;
using CovaldysPilot.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CovaldysPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController(
    IReviewService reviewService,
    ILogger<ReviewController> logger) : ControllerBase
{
    [HttpGet("event/{eventId:guid}")]
    [AllowAnonymous]
    [EndpointSummary("Récupérer les avis d'un événement")]
    public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetByEvent(Guid eventId)
    {
        logger.LogInformation("GET /api/reviews/event/{EventId}", eventId);
        IEnumerable<ReviewResponseDto> reviews = await reviewService.GetByEventAsync(eventId);
        return Ok(reviews);
    }

    [HttpPost]
    [Authorize]
    [EndpointSummary("Laisser un avis sur un événement terminé")]
    public async Task<ActionResult<ReviewResponseDto>> Create([FromBody] CreateReviewRequestDto dto)
    {
        Guid? userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();

        logger.LogInformation("POST /api/reviews - UserId: {UserId}", userId);
        ReviewResponseDto review = await reviewService.CreateAsync(userId.Value, dto);
        return CreatedAtAction(nameof(GetByEvent), new { eventId = review.EventId }, review);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    [EndpointSummary("Modifier son avis")]
    public async Task<ActionResult<ReviewResponseDto>> Update(Guid id, [FromBody] UpdateReviewRequestDto dto)
    {
        Guid? userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();

        logger.LogInformation("PUT /api/reviews/{Id}", id);
        ReviewResponseDto review = await reviewService.UpdateAsync(userId.Value, id, dto);
        return Ok(review);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    [EndpointSummary("Supprimer son avis")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Guid? userId = GetCurrentUserId();
        if (userId is null) return Unauthorized();

        logger.LogInformation("DELETE /api/reviews/{Id}", id);
        await reviewService.DeleteAsync(userId.Value, id);
        return NoContent();
    }

    private Guid? GetCurrentUserId()
    {
        string? userIdClaim = User.FindFirstValue("sub");
        return Guid.TryParse(userIdClaim, out Guid userId) ? userId : null;
    }
}