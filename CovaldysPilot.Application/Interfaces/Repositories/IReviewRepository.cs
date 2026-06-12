using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

/// <inheritdoc/>
public interface IReviewRepository : IBaseRepository<Review>
{
  #region GetByEventAsync
  /// <summary>
  /// Récupère tous les avis laissés pour un événement spécifique de manière asynchrone.
  /// </summary>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>Une collection d'avis de type <see cref="Review"/> associés à cet événement.</returns>
  Task<IEnumerable<Review>> GetByEventAsync(Guid eventId);
  #endregion

  #region GetByUserAndEventAsync
  /// <summary>
  /// Récupère l'avis rédigé par un utilisateur pour un événement spécifique de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur.</param>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>L'avis de type <see cref="Review"/> correspondant, ou <see langword="null"/> si aucun avis n'existe pour cet utilisateur sur cet événement.</returns>
  Task<Review?> GetByUserAndEventAsync(Guid userId, Guid eventId);
  #endregion
}