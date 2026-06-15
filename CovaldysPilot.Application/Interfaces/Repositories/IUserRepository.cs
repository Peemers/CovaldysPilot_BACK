using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

/// <inheritdoc/>
public interface IUserRepository : IBaseRepository<User>
{
  #region GetByEmailAsync
  /// <summary>
  /// Récupère un utilisateur par son adresse e-mail de manière asynchrone.
  /// </summary>
  /// <param name="email">L'adresse e-mail de l'utilisateur.</param>
  /// <returns>L'utilisateur de type <see cref="User"/> correspondant, ou <see langword="null"/> si aucun utilisateur n'est trouvé.</returns>
  Task<User?> GetByEmailAsync(string email);
  #endregion

  #region GetByEmailOrPseudoAsync
  /// <summary>
  /// Récupère un utilisateur par son adresse e-mail ou son pseudonyme de manière asynchrone.
  /// </summary>
  /// <param name="emailOrPseudo">L'adresse e-mail ou le pseudonyme de l'utilisateur.</param>
  /// <returns>L'utilisateur de type <see cref="User"/> correspondant, ou <see langword="null"/> si aucun utilisateur n'est trouvé.</returns>
  Task<User?> GetByEmailOrPseudoAsync(string emailOrPseudo);
  #endregion

  #region EmailExistsAsync
  /// <summary>
  /// Vérifie si une adresse e-mail est déjà utilisée par un utilisateur de manière asynchrone.
  /// </summary>
  /// <param name="email">L'adresse e-mail à vérifier.</param>
  /// <returns><see langword="true"/> si l'adresse e-mail existe ; sinon, <see langword="false"/>.</returns>
  Task<bool> EmailExistsAsync(string email);
  #endregion

  #region PseudoExistsAsync
  /// <summary>
  /// Vérifie si un pseudonyme est déjà utilisé par un utilisateur de manière asynchrone.
  /// </summary>
  /// <param name="pseudo">Le pseudonyme à vérifier.</param>
  /// <returns><see langword="true"/> si le pseudonyme existe ; sinon, <see langword="false"/>.</returns>
  Task<bool> PseudoExistsAsync(string pseudo);
  #endregion
}