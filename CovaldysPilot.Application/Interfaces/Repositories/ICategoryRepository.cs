using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

/// <inheritdoc/>
public interface ICategoryRepository : IBaseRepository<Category>
{
  #region NameExistsAsync
  /// <summary>
  /// Vérifie si une catégorie existe déjà avec le nom spécifié de manière asynchrone.
  /// </summary>
  /// <param name="name">Le nom de la catégorie à vérifier.</param>
  /// <returns><see langword="true"/> si la catégorie existe ; sinon, <see langword="false"/>.</returns>
  Task<bool> NameExistsAsync(string name);
  #endregion

  #region GetByNameAsync
  /// <summary>
  /// Récupère une catégorie par son nom de manière asynchrone.
  /// </summary>
  /// <param name="name">Le nom de la catégorie à récupérer.</param>
  /// <returns>La catégorie de type <see cref="Category"/> correspondante, ou <see langword="null"/> si elle n'existe pas.</returns>
  Task<Category?> GetByNameAsync(string name);
  #endregion
}