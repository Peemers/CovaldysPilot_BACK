using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

/// <inheritdoc/>
public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
  #region GetByTokenAsync
  /// <summary>
  /// Récupère un jeton de rafraîchissement par sa valeur de manière asynchrone.
  /// </summary>
  /// <param name="token">La valeur textuelle du jeton de rafraîchissement.</param>
  /// <returns>Le jeton de rafraîchissement de type <see cref="RefreshToken"/> correspondant, ou <see langword="null"/> si elle n'existe pas.</returns>
  Task<RefreshToken?> GetByTokenAsync(string token);
  #endregion

  #region RevokeTokenAsync
  /// <summary>
  /// Révoque un jeton de rafraîchissement de manière asynchrone.
  /// </summary>
  /// <param name="token">La valeur textuelle du jeton de rafraîchissement à révoquer.</param>
  /// <returns>Une <see cref="Task"/> représentant l'opération asynchrone.</returns>
  Task RevokeTokenAsync(string token);
  #endregion
}