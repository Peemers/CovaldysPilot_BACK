using CovaldysPilot.Application.DTOs.Article.Request;
using CovaldysPilot.Application.DTOs.Article.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IArticleService
{
  #region GetAllAsync
  /// <summary>
  /// Récupère tous les articles de manière asynchrone.
  /// </summary>
  /// <returns>Une collection de DTO de réponse contenant les informations des articles.</returns>
  Task<IEnumerable<ArticleResponseDto>> GetAllAsync();
  #endregion

  #region GetByIdAsync
  /// <summary>
  /// Récupère un article par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'article.</param>
  /// <returns>Le DTO de réponse correspondant à l'article, ou <see langword="null"/> si l'article n'existe pas.</returns>
  Task<ArticleResponseDto?> GetByIdAsync(Guid id);
  #endregion

  #region CreateAsync
  /// <summary>
  /// Crée un nouvel article de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les données de création de l'article.</param>
  /// <param name="userId">L'identifiant unique de l'utilisateur créateur, ou <see langword="null"/>.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'article créé.</returns>
  Task<ArticleResponseDto> CreateAsync(CreateArticleRequestDto dto, Guid? userId);
  #endregion

  #region UpdateAsync
  /// <summary>
  /// Met à jour un article existant de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'article à modifier.</param>
  /// <param name="dto">Le DTO contenant les données de mise à jour de l'article.</param>
  /// <returns>Le DTO de réponse contenant les détails de l'article mis à jour.</returns>
  Task<ArticleResponseDto> UpdateAsync(Guid id, UpdateArticleRequestDto dto);
  #endregion

  #region DeleteAsync
  /// <summary>
  /// Supprime un article par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'article à supprimer.</param>
  /// <returns>Une tâche représentant l'opération de suppression asynchrone.</returns>
  Task DeleteAsync(Guid id);
  #endregion

  #region AddImageAsync
  /// <summary>
  /// Ajoute une image à un article existant de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de l'article auquel ajouter l'image.</param>
  /// <param name="imageUrl">L'URL de l'image à ajouter.</param>
  /// <returns>Le DTO de réponse de l'article mis à jour.</returns>
  Task<ArticleResponseDto> AddImageAsync(Guid id, string imageUrl);
  #endregion

  #region DeleteImageAsync
  /// <summary>
  /// Supprime une image d'un article de manière asynchrone.
  /// </summary>
  /// <param name="articleId">L'identifiant unique de l'article.</param>
  /// <param name="imageId">L'identifiant unique de l'image à supprimer.</param>
  /// <returns>Une tâche représentant l'opération de suppression asynchrone.</returns>
  Task DeleteImageAsync(Guid articleId, Guid imageId);
  #endregion
}