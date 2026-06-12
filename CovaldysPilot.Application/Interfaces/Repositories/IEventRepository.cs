using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.Interfaces.Repositories;

/// <inheritdoc/>
public interface IEventRepository : IBaseRepository<Event>
{
  #region GetAllWithCategoriesAsync
  /// <summary>
  /// Récupère tous les événements en incluant leurs catégories associées de manière asynchrone.
  /// </summary>
  /// <returns>Une collection de tous les événements de type <see cref="Event"/>.</returns>
  Task<IEnumerable<Event>> GetAllWithCategoriesAsync();
  #endregion

  #region GetByIdWithDetailsAsync
  /// <summary>
  /// Récupère un événement avec tous ses détails par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'événement.</param>
  /// <returns>L'entité <see cref="Event"/> correspondante, ou <see langword="null"/> si elle n'existe pas.</returns>
  Task<Event?> GetByIdWithDetailsAsync(Guid id);
  #endregion

  #region GetByStatusAsync
  /// <summary>
  /// Récupère les événements filtrés par leur statut de manière asynchrone.
  /// </summary>
  /// <param name="status">Le statut <see cref="EventStatus"/> de l'événement.</param>
  /// <returns>Une collection d'événements de type <see cref="Event"/> filtrés par le statut spécifié.</returns>
  Task<IEnumerable<Event>> GetByStatusAsync(EventStatus status);
  #endregion

  #region GetCurrentParticipantsCountAsync
  /// <summary>
  /// Récupère le nombre actuel de participants inscrits pour un événement spécifique de manière asynchrone.
  /// </summary>
  /// <param name="eventId">L'identifiant unique de l'événement.</param>
  /// <returns>Le nombre de participants inscrits.</returns>
  Task<int> GetCurrentParticipantsCountAsync(Guid eventId);
  #endregion

  #region AnyByCategoryIdAsync
  /// <summary>
  /// Vérifie s'il existe au moins un événement associé à la catégorie spécifiée de manière asynchrone.
  /// </summary>
  /// <param name="categoryId">L'identifiant unique de la catégorie.</param>
  /// <returns><see langword="true"/> si au moins un événement est associé à cette catégorie ; sinon, <see langword="false"/>.</returns>
  Task<bool> AnyByCategoryIdAsync(Guid categoryId);
  #endregion
}