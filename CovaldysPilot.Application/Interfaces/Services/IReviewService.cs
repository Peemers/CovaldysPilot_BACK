using CovaldysPilot.Application.DTOs.Review.Request;
using CovaldysPilot.Application.DTOs.Review.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IReviewService
{
  #region GetByEventAsync
  /// <summary>
  /// Récupère tous les avis pour un événement spécifique de manière asynchrone.
  /// </summary>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>Une collection de DTO de réponse contenant les avis associés à l'événement.</returns>
  Task<IEnumerable<ReviewResponseDto>> GetByEventAsync(Guid eventId);
  #endregion

  #region CreateAsync
  /// <summary>
  /// Crée un nouvel avis pour un événement de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur qui crée l'avis.</param>
  /// <param name="dto">Le DTO contenant les données de création de l'avis.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'avis créé.</returns>
  Task<ReviewResponseDto> CreateAsync(Guid userId, CreateReviewRequestDto dto);
  #endregion

  #region UpdateAsync
  /// <summary>
  /// Met à jour un avis existant de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur qui met à jour l'avis.</param>
  /// <param name="reviewId">L'identifiant unique de l'avis à modifier.</param>
  /// <param name="dto">Le DTO contenant les nouvelles données de l'avis.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'avis mis à jour.</returns>
  Task<ReviewResponseDto> UpdateAsync(Guid userId, Guid reviewId, UpdateReviewRequestDto dto);
  #endregion

  #region DeleteAsync
  /// <summary>
  /// Supprime un avis de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur qui supprime l'avis.</param>
  /// <param name="reviewId">L'identifiant unique de l'avis à supprimer.</param>
  /// <returns>Une tâche représentant l'opération de suppression asynchrone.</returns>
  Task DeleteAsync(Guid userId, Guid reviewId);
  #endregion
}