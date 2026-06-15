using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

/// <inheritdoc/>
public interface ISignInRepository : IBaseRepository<SignIn>
{
  #region GetByUserAndEventAsync
  /// <summary>
  /// Récupère l'inscription d'un utilisateur pour un événement spécifique de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur.</param>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>L'inscription de type <see cref="SignIn"/> correspondante, ou <see langword="null"/> si l'utilisateur n'est pas inscrit à cet événement.</returns>
  Task<SignIn?> GetByUserAndEventAsync(Guid userId, Guid eventId);
  #endregion

  #region GetByEventAsync
  /// <summary>
  /// Récupère toutes les inscriptions pour un événement spécifique de manière asynchrone.
  /// </summary>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>Une collection d'inscriptions de type <see cref="SignIn"/> associées à cet événement.</returns>
  Task<IEnumerable<SignIn>> GetByEventAsync(Guid eventId);
  #endregion

  #region GetByUserAsync
  /// <summary>
  /// Récupère toutes les inscriptions d'un utilisateur de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur.</param>
  /// <returns>Une collection d'inscriptions de type <see cref="SignIn"/> associées à cet utilisateur.</returns>
  Task<IEnumerable<SignIn>> GetByUserAsync(Guid userId);
  #endregion

  #region GetFirstOnWaitingListAsync
  /// <summary>
  /// Récupère la première inscription sur la liste d'attente pour un événement spécifique de manière asynchrone.
  /// </summary>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>L'inscription de type <see cref="SignIn"/> correspondante au premier élément sur la liste d'attente, ou <see langword="null"/> si la liste d'attente est vide.</returns>
  Task<SignIn?> GetFirstOnWaitingListAsync(Guid eventId);
  #endregion

  #region GetWaitingListCountAsync
  /// <summary>
  /// Récupère le nombre d'inscriptions actuellement en liste d'attente pour un événement spécifique de manière asynchrone.
  /// </summary>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>Le nombre d'inscriptions sur la liste d'attente.</returns>
  Task<int> GetWaitingListCountAsync(Guid eventId);
  #endregion
}