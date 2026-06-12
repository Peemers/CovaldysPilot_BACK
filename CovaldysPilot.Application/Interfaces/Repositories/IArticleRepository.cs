using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

/// <inheritdoc/>
public interface IArticleRepository : IBaseRepository<Article>
{
  #region GetAllArticlesWhitImageAsync
  /// <summary>
  /// Récupère tous les articles avec leurs images associées de manière asynchrone.
  /// </summary>
  /// <returns>Une collection de tous les articles de type <see cref="Article"/>.</returns>
  Task<IEnumerable<Article>> GetAllArticlesWhitImageAsync();
  #endregion

  #region GetByIdWithImageAsync
  /// <summary>
  /// Récupère un article par son identifiant unique en incluant ses images associées de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'article.</param>
  /// <returns>L'entité <see cref="Article"/> correspondante, ou <see langword="null"/> si elle n'existe pas.</returns>
  Task<Article?> GetByIdWithImageAsync(Guid id);
  #endregion

  #region AddImageAsync
  /// <summary>
  /// Ajoute une image à un article de manière asynchrone.
  /// </summary>
  /// <param name="image">L'image <see cref="ArticleImage"/> à ajouter.</param>
  /// <returns>Une <see cref="Task"/> représentant l'opération asynchrone.</returns>
  Task AddImageAsync(ArticleImage image);
  #endregion
}