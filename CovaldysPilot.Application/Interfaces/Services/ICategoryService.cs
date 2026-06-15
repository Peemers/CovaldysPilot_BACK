using CovaldysPilot.Application.DTOs.Category.Request;
using CovaldysPilot.Application.DTOs.Category.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface ICategoryService
{
  #region GetAllAsync
  /// <summary>
  /// Récupère toutes les catégories de manière asynchrone.
  /// </summary>
  /// <returns>Une collection de DTO de réponse contenant les informations des catégories.</returns>
  Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
  #endregion

  #region CreateAsync
  /// <summary>
  /// Crée une nouvelle catégorie de manière asynchrone.
  /// </summary>
  /// <param name="dto">Le DTO contenant les données de création de la catégorie.</param>
  /// <returns>Le DTO de réponse contenant les détails de la catégorie créée.</returns>
  Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto dto);
  #endregion

  #region DeleteAsync
  /// <summary>
  /// Supprime une catégorie par son identifiant unique de manière asynchrone.
  /// </summary>
  /// <param name="id">L'identifiant unique de la catégorie à supprimer.</param>
  /// <returns>Une tâche représentant l'opération de suppression asynchrone.</returns>
  Task DeleteAsync(Guid id);
  #endregion
}