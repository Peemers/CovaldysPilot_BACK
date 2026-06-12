using CovaldysPilot.Application.DTOs.SignIn.Request;
using CovaldysPilot.Application.DTOs.SignIn.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface ISignInService
{
  #region RegisterAsync
  /// <summary>
  /// Inscrit un utilisateur à un événement de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur à inscrire.</param>
  /// <param name="dto">Le DTO contenant les données nécessaires à l'inscription.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'inscription.</returns>
  Task<SignInResponseDto> RegisterAsync(Guid userId, CreateSignInRequestDto dto);
  #endregion

  #region UnregisterAsync
  /// <summary>
  /// Désinscrit un utilisateur d'un événement de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur.</param>
  /// <param name="signInId">L'identifiant unique de l'inscription à annuler.</param>
  /// <returns>Une tâche représentant l'opération de désinscription asynchrone.</returns>
  Task UnregisterAsync(Guid userId, Guid signInId);
  #endregion

  #region GetByEventAsync
  /// <summary>
  /// Récupère toutes les inscriptions pour un événement spécifique de manière asynchrone.
  /// </summary>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>Une collection de DTO de réponse contenant les inscriptions associées à l'événement.</returns>
  Task<IEnumerable<SignInResponseDto>> GetByEventAsync(Guid eventId);
  #endregion

  #region GetByUserAsync
  /// <summary>
  /// Récupère toutes les inscriptions d'un utilisateur de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur.</param>
  /// <returns>Une collection de DTO de réponse contenant les inscriptions de l'utilisateur.</returns>
  Task<IEnumerable<SignInResponseDto>> GetByUserAsync(Guid userId);
  #endregion

  #region ValidatePayment
  /// <summary>
  /// Valide le paiement d'une inscription de manière asynchrone.
  /// </summary>
  /// <param name="signInId">L'identifiant unique de l'inscription dont le paiement doit être validé.</param>
  /// <returns>Une tâche représentant l'opération de validation asynchrone.</returns>
  Task ValidatePayment(Guid signInId);
  #endregion
  
  //administration

  #region AdminRegisterAsync
  /// <summary>
  /// Inscrit un utilisateur à un événement via une action administrative de manière asynchrone.
  /// </summary>
  /// <param name="userId">L'identifiant unique de l'utilisateur à inscrire.</param>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'inscription administrative.</returns>
  Task<SignInResponseDto> AdminRegisterAsync(Guid userId, Guid eventId);
  #endregion

  #region AdminUnregisterAsync
  /// <summary>
  /// Annule une inscription via une action administrative de manière asynchrone.
  /// </summary>
  /// <param name="signInId">L'identifiant unique de l'inscription à annuler.</param>
  /// <returns>Une tâche représentant l'opération de désinscription administrative asynchrone.</returns>
  Task AdminUnregisterAsync(Guid signInId);
  #endregion
}