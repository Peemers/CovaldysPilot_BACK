using CovaldysPilot.Application.DTOs.Event.Request;
using CovaldysPilot.Application.DTOs.Event.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IEventService
{
  #region GetAllAsync
  /// <summary>
  /// Récupère tous les événements de manière asynchrone.
  /// </summary>
  /// <param name="currentUserId">L'identifiant unique de l'utilisateur connecté actuellement, ou <see langword="null"/>.</param>
  /// <returns>Une collection de DTO de réponse contenant les informations des événements.</returns>
  Task<IEnumerable<EventResponseDto>> GetAllAsync(Guid? currentUserId = null);
  #endregion

  #region GetByIdAsync
  /// <summary>
  /// Récupère un événement par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement.</param>
  /// <param name="currentUserId">L'identifiant unique de l'utilisateur connecté actuellement, ou <see langword="null"/>.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'événement, ou <see langword="null"/> si l'événement n'existe pas.</returns>
  Task<EventResponseDto?> GetByIdAsync(Guid id, Guid? currentUserId = null);
  #endregion

  #region CreateAsync
  /// <summary>
  /// Crée un nouvel événement de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les données de création de l'événement.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'événement créé.</returns>
  Task<EventResponseDto> CreateAsync(CreateEventRequestDto dto);
  #endregion

  #region UpdateAsync
  /// <summary>
  /// Met à jour un événement existant de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement à modifier.</param>
  /// <param name="dto">Le DTO contenant les données de mise à jour de l'événement.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'événement mis à jour.</returns>
  Task<EventResponseDto> UpdateAsync(Guid id, UpdateEventRequestDto dto);
  #endregion

  #region DeleteAsync
  /// <summary>
  /// Supprime un événement par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement à supprimer.</param>
  /// <returns>Une tâche représentant l'opération de suppression asynchrone.</returns>
  Task DeleteAsync(Guid id);
  #endregion

  #region CancelAsync
  /// <summary>
  /// Annule un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement à annuler.</param>
  /// <param name="cancellationReason">Le motif d'annulation de l'événement.</param>
  /// <returns>Une tâche représentant l'opération d'annulation asynchrone.</returns>
  Task CancelAsync(Guid id, string? cancellationReason = null);
  #endregion

  #region StartAsync
  /// <summary>
  /// Démarre un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement à démarrer.</param>
  /// <returns>Une tâche représentant l'opération de démarrage asynchrone.</returns>
  Task StartAsync(Guid id);
  #endregion

  #region CloseAsync
  /// <summary>
  /// Clôture un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement à clôturer.</param>
  /// <returns>Une tâche représentant l'opération de clôture asynchrone.</returns>
  Task CloseAsync(Guid id);
  #endregion

  #region GetStatsAsync
  /// <summary>
  /// Récupère les statistiques d'un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement.</param>
  /// <returns>Le DTO contenant les statistiques de l'événement.</returns>
  Task<EventStatsResponseDto> GetStatsAsync(Guid id);
  #endregion

  #region SendReminderAsync
  /// <summary>
  /// Envoie un rappel pour un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement pour lequel envoyer un rappel.</param>
  /// <returns>Une tâche représentant l'opération d'envoi du rappel asynchrone.</returns>
  Task SendReminderAsync(Guid id);
  #endregion

  #region UpdateCoverImageAsync
  /// <summary>
  /// Met à jour l'image de couverture d'un événement de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement.</param>
  /// <param name="imageUrl">L'URL de la nouvelle image de couverture.</param>
  /// <returns>Une tâche représentant l'opération de mise à jour asynchrone.</returns>
  Task UpdateCoverImageAsync(Guid id, string imageUrl);
  #endregion
}