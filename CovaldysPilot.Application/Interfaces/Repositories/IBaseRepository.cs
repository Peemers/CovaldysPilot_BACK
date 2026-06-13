using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

/// <summary>
/// Interface de base pour les dépôts (repositories) gérant les opérations CRUD de base.
/// </summary>
/// <typeparam name="T">Le type de l'entité gérée, héritant de <see cref="BaseEntity"/>.</typeparam>
public interface IBaseRepository<T> where T : BaseEntity
{
  #region GetByIdAsync
  /// <summary>
  /// Récupère une entité par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'entité.</param>
  /// <returns>L'entité de type <typeparamref name="T"/> correspondante, ou <see langword="null"/> si elle n'est pas trouvée.</returns>
  Task<T?> GetByIdAsync(Guid id);
  #endregion

  #region GetAllAsync
  /// <summary>
  /// Récupère toutes les entités de manière asynchrone.
  /// </summary>
  /// <returns>Une collection de toutes les entités de type <typeparamref name="T"/>.</returns>
  Task<IEnumerable<T>> GetAllAsync();
  #endregion

  #region AddAsync
  /// <summary>
  /// Ajoute une nouvelle entité de manière asynchrone.
  /// </summary>
  /// <param name="entity">L'entité à ajouter.</param>
  /// <returns>Une <see cref="Task"/> représentant l'opération asynchrone.</returns>
  Task AddAsync(T entity);
  #endregion

  #region UpdateAsync
  /// <summary>
  /// Met à jour une entité existante de manière asynchrone.
  /// </summary>
  /// <param name="entity">L'entité contenant les modifications à appliquer.</param>
  /// <returns>Une <see cref="Task"/> représentant l'opération asynchrone.</returns>
  Task UpdateAsync(T entity);
  #endregion

  #region DeleteAsync
  /// <summary>
  /// Supprime une entité par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'entité à supprimer.</param>
  /// <returns>Une <see cref="Task"/> représentant l'opération asynchrone.</returns>
  Task DeleteAsync(Guid id);
  #endregion

  #region SaveChangesAsync
  /// <summary>
  /// Enregistre les modifications en attente dans la base de données de manière asynchrone.
  /// </summary>
  /// <returns>Une <see cref="Task"/> représentant l'opération asynchrone.</returns>
  Task SaveChangesAsync();
  #endregion
}