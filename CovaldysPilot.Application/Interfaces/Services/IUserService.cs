using CovaldysPilot.Application.DTOs.User.Request;
using CovaldysPilot.Application.DTOs.User.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IUserService
{
  #region GetAllAsync
  /// <summary>
  /// Récupère tous les utilisateurs de manière asynchrone.
  /// </summary>
  /// <returns>Une collection de DTO de réponse contenant les informations des utilisateurs.</returns>
  Task<IEnumerable<UserResponseDto>> GetAllAsync();
  #endregion

  #region GetByIdAsync
  /// <summary>
  /// Récupère un utilisateur par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'utilisateur.</param>
  /// <returns>Le DTO de réponse de l'utilisateur s'il existe, sinon <see langword="null"/>.</returns>
  Task<UserResponseDto?> GetByIdAsync(Guid id);
  #endregion

  #region DeleteAsync
  /// <summary>
  /// Supprime un utilisateur par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'utilisateur à supprimer.</param>
  /// <returns>Une tâche représentant l'opération de suppression asynchrone.</returns>
  Task DeleteAsync(Guid id);
  #endregion

  #region ExportMembersAsync
  /// <summary>
  /// Exporte la liste des membres de manière asynchrone.
  /// </summary>
  /// <param name="filter">Le filtre optionnel à appliquer lors de l'export.</param>
  /// <returns>Un tableau d'octets représentant le fichier exporté.</returns>
  Task<byte[]> ExportMembersAsync(string? filter = null);
  #endregion

  #region AddManuallyAsync
  /// <summary>
  /// Ajoute manuellement un nouvel utilisateur de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les informations pour la création manuelle de l'utilisateur.</param>
  /// <returns>Le DTO de réponse contenant les informations de l'utilisateur créé.</returns>
  Task<CreateUserManuallyResponseDto> AddManuallyAsync(CreateUserManuallyRequestDto dto);
  #endregion
  
  #region PayCotisationAsync
  /// <summary>
  /// Simule le paiement de la cotisation annuelle de 10€ pour un membre de manière asynchrone.
  /// Met à jour le statut du membre en "Effectif" et enregistre la date du dernier paiement.
  /// </summary>
  /// <param name="userId">L'identifiant unique du membre.</param>
  /// <returns>Une tâche asynchrone représentant l'opération.</returns>
  Task PayCotisationAsync(Guid userId);
  #endregion
}