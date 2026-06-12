namespace CovaldysPilot.Application.DTOs.User.Response;

/// <summary>
/// Données de réponse après la création manuelle d'un utilisateur, contenant ses informations et son mot de passe temporaire.
/// </summary>
public class CreateUserManuallyResponseDto
{
  /// <summary>
  /// L'identifiant unique de l'utilisateur.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// Le pseudonyme unique de l'utilisateur.
  /// </summary>
  public required string Pseudo { get; init; }

  /// <summary>
  /// L'adresse e-mail de l'utilisateur.
  /// </summary>
  public required string Email { get; init; }

  /// <summary>
  /// Le prénom de l'utilisateur.
  /// </summary>
  public required string FirstName { get; init; }

  /// <summary>
  /// Le nom de famille de l'utilisateur.
  /// </summary>
  public string? LastName { get; init; }

  /// <summary>
  /// Indique si la cotisation de l'utilisateur est à jour.
  /// </summary>
  public bool IsMembershipUpToDate { get; init; }

  //  UNE SEULE FOIS pour l'admin, jamais stock en clair en DB !!!!!
  /// <summary>
  /// Le mot de passe temporaire généré pour l'utilisateur.
  /// </summary>
  public required string TemporaryPassword { get; init; }
}