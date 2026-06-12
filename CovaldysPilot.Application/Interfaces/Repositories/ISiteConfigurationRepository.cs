using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

/// <summary>
/// Interface du dépôt gérant la configuration globale du site.
/// </summary>
public interface ISiteConfigurationRepository
{
  #region GetAsync
  /// <summary>
  /// Récupère la configuration globale du site de manière asynchrone.
  /// </summary>
  /// <returns>La configuration du site sous la forme d'un objet <see cref="SiteConfiguration"/>.</returns>
  Task<SiteConfiguration> GetAsync();
  #endregion

  #region UpdateAsync
  /// <summary>
  /// Met à jour la configuration globale du site de manière asynchrone.
  /// </summary>
  /// <param name="config">Les nouvelles informations de configuration de type <see cref="SiteConfiguration"/> à appliquer.</param>
  /// <returns>Une <see cref="Task"/> représentant l'opération asynchrone.</returns>
  Task UpdateAsync(SiteConfiguration config);
  #endregion

  #region SaveChangesAsync
  /// <summary>
  /// Enregistre les modifications en attente dans la base de données de manière asynchrone.
  /// </summary>
  /// <returns>Une <see cref="Task"/> représentant l'opération asynchrone.</returns>
  Task SaveChangesAsync();
  #endregion
}