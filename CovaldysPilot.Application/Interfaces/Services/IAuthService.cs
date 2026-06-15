using CovaldysPilot.Application.DTOs.Auth.Request;
using CovaldysPilot.Application.DTOs.Auth.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IAuthService
{
  #region RegisterAsync
  /// <summary>
  /// Enregistre un nouvel utilisateur de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les données d'inscription de l'utilisateur.</param>
  /// <returns>Le DTO de réponse contenant les informations d'authentification et les jetons générés.</returns>
  Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
  #endregion

  #region LoginAsync
  /// <summary>
  /// Authentifie un utilisateur de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les identifiants de connexion de l'utilisateur.</param>
  /// <returns>Le DTO de réponse contenant les informations d'authentification et les jetons générés.</returns>
  Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
  #endregion

  #region RefreshTokenAsync
  /// <summary>
  /// Rafraîchit le jeton d'accès à l'aide d'un jeton de rafraîchissement de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant le jeton de rafraîchissement.</param>
  /// <returns>Le DTO de réponse contenant le nouveau jeton d'accès et de rafraîchissement.</returns>
  Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto);
  #endregion

  #region RevokeTokenAsync
  /// <summary>
  /// Révoque un jeton de rafraîchissement de manière asynchrone.
  /// </summary>
  /// <param name="refreshToken">Le jeton de rafraîchissement à révoquer.</param>
  /// <returns>Une tâche représentant l'opération de révocation asynchrone.</returns>
  Task RevokeTokenAsync(string refreshToken);
  #endregion

  #region ChangePasswordAsync
  /// <summary>
  /// Modifie le mot de passe d'un utilisateur de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur.</param>
  /// <param name="dto">Le DTO contenant l'ancien et le nouveau mot de passe.</param>
  /// <returns>Une tâche représentant l'opération de modification asynchrone.</returns>
  Task ChangePasswordAsync(Guid userId, ChangePasswordRequestDto dto);
  #endregion
}