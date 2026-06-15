using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IJwtService
{
  #region GenerateAccessToken
  /// <summary>
  /// Génère un jeton d'accès (JWT) pour un utilisateur spécifié.
  /// </summary>
  /// <param name="user">L'utilisateur pour lequel générer le jeton.</param>
  /// <returns>Le jeton d'accès sous forme de chaîne de caractères.</returns>
  string GenerateAccessToken(User user);
  #endregion

  #region GenerateRefreshToken
  /// <summary>
  /// Génère un nouveau jeton de rafraîchissement.
  /// </summary>
  /// <returns>Le jeton de rafraîchissement sous forme de chaîne de caractères.</returns>
  string GenerateRefreshToken();
  #endregion

  #region GetRefreshTokenExpiryDate
  /// <summary>
  /// Récupère la date d'expiration pour un nouveau jeton de rafraîchissement.
  /// </summary>
  /// <returns>La date et l'heure d'expiration du jeton de rafraîchissement.</returns>
  DateTime GetRefreshTokenExpiryDate();
  #endregion
}