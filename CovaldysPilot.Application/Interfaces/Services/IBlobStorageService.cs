namespace CovaldysPilot.Application.Interfaces.Services;

public interface IBlobStorageService
{
  #region UploadAsync
  /// <summary>
  /// Téléverse un fichier dans le stockage de blobs de manière asynchrone.
  /// </summary>
  /// <param name="fileStream">Le flux contenant les données du fichier à téléverser.</param>
  /// <param name="fileName">Le nom du fichier.</param>
  /// <param name="contentType">Le type de contenu MIME du fichier.</param>
  /// <returns>L'URL du fichier téléversé.</returns>
  Task<string> UploadAsync (Stream fileStream, string fileName, string contentType);
  #endregion

  #region DeleteAsync
  /// <summary>
  /// Supprime un fichier du stockage de blobs de manière asynchrone.
  /// </summary>
  /// <param name="fileUrl">L'URL du fichier à supprimer.</param>
  /// <returns>Une tâche représentant l'opération de suppression asynchrone.</returns>
  Task DeleteAsync (string fileUrl);
  #endregion
}