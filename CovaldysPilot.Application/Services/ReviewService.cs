using CovaldysPilot.Application.DTOs.Review.Request;
using CovaldysPilot.Application.DTOs.Review.Response;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Mappers;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace CovaldysPilot.Application.Services;

public class ReviewService(
    IReviewRepository reviewRepository,
    IEventRepository eventRepository,
    ILogger<ReviewService> logger) : IReviewService
{
    public async Task<IEnumerable<ReviewResponseDto>> GetByEventAsync(Guid eventId)
    {
        logger.LogInformation("Récupération des avis pour l'événement {EventId}", eventId);
        IEnumerable<Review> reviews = await reviewRepository.GetByEventAsync(eventId);
        return reviews.Select(r => r.ToReviewResponseDto());
    }

    public async Task<ReviewResponseDto> CreateAsync(Guid userId, CreateReviewRequestDto dto)
    {
        logger.LogInformation("Création d'un avis par {UserId} pour l'événement {EventId}", userId, dto.EventId);

        // Vérif event existe et est termine
        Event? evt = await eventRepository.GetByIdAsync(dto.EventId);
        if (evt is null)
            throw new KeyNotFoundException($"Événement {dto.EventId} introuvable.");
        if (evt.Status != EventStatus.Termine)
            throw new InvalidOperationException("Vous ne pouvez laisser un avis que sur un événement terminé.");

        // Verif si note entre 1 et 5
        if (dto.Note < 1 || dto.Note > 5)
            throw new InvalidOperationException("La note doit être comprise entre 1 et 5.");

        // Vérif si pas déjà un avis
        Review? existing = await reviewRepository.GetByUserAndEventAsync(userId, dto.EventId);
        if (existing != null)
            throw new InvalidOperationException("Vous avez déjà laissé un avis pour cet événement.");

        Review review = new Review //idem que partout ailleur, mapping ici car 2 id
        {
            UserId = userId,
            EventId = dto.EventId,
            Note = dto.Note,
            Comment = dto.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await reviewRepository.AddAsync(review);
        await reviewRepository.SaveChangesAsync();

        // Recharger avec User pour le pseudo
        Review? created = await reviewRepository.GetByUserAndEventAsync(userId, dto.EventId);
        logger.LogInformation("Avis créé par {UserId} pour {EventId}", userId, dto.EventId);
        return created!.ToReviewResponseDto();
    }

    public async Task<ReviewResponseDto> UpdateAsync(Guid userId, Guid reviewId, UpdateReviewRequestDto dto)
    {
        logger.LogInformation("Modification de l'avis {ReviewId} par {UserId}", reviewId, userId);

        Review? review = await reviewRepository.GetByIdAsync(reviewId);
        if (review is null)
            throw new KeyNotFoundException($"Avis {reviewId} introuvable.");
        if (review.UserId != userId)
            throw new InvalidOperationException("Vous ne pouvez modifier que vos propres avis.");
        if (dto.Note < 1 || dto.Note > 5)
            throw new InvalidOperationException("La note doit être comprise entre 1 et 5.");

        review.Note = dto.Note;
        review.Comment = dto.Comment;
        review.UpdatedAt = DateTime.UtcNow;

        await reviewRepository.UpdateAsync(review);
        await reviewRepository.SaveChangesAsync();

        logger.LogInformation("Avis modifié : {ReviewId}", reviewId);
        return review.ToReviewResponseDto();
    }

    public async Task DeleteAsync(Guid userId, Guid reviewId)
    {
        logger.LogInformation("Suppression de l'avis {ReviewId} par {UserId}", reviewId, userId);

        Review? review = await reviewRepository.GetByIdAsync(reviewId);
        if (review is null)
            throw new KeyNotFoundException($"Avis {reviewId} introuvable.");
        if (review.UserId != userId)
            throw new InvalidOperationException("Vous ne pouvez supprimer que vos propres avis.");

        await reviewRepository.DeleteAsync(reviewId);
        await reviewRepository.SaveChangesAsync();
        logger.LogInformation("Avis supprimé : {ReviewId}", reviewId);
    }
}