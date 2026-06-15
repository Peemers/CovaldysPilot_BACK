namespace CovaldysPilot.Application.DTOs.Auth.Response;

/// <summary>
/// Données de réponse après une authentification réussie.
/// </summary>
public class AuthResponseDto
{
  /// <summary>
  /// L'identifiant unique de l'utilisateur.
  /// </summary>
  public required Guid UserId { get; init; }

  /// <summary>
  /// Le jeton d'accès JWT.
  /// </summary>
  public required string AccessToken { get; init; }

  /// <summary>
  /// Le jeton de rafraîchissement.
  /// </summary>
  public required string RefreshToken { get; init; }

  /// <summary>
  /// Le pseudonyme de l'utilisateur.
  /// </summary>
  public required string Pseudo { get; init; }

  /// <summary>
  /// Le rôle de l'utilisateur.
  /// </summary>
  public required string Role { get; init; }

  /// <summary>
  /// La date et l'heure d'expiration du jeton d'accès.
  /// </summary>
  public required DateTime ExpiresAt { get; init; }

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
}