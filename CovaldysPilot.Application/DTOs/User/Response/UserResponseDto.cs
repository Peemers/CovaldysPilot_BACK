using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.DTOs.User.Response;

/// <summary>
/// Données de réponse représentant les informations détaillées d'un utilisateur.
/// </summary>
public class UserResponseDto
{
  /// <summary>
  /// L'identifiant unique de l'utilisateur.
  /// </summary>
  public Guid Id { get; init; }

  /// <summary>
  /// Le pseudonyme de l'utilisateur.
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
  /// Le numéro de téléphone de l'utilisateur.
  /// </summary>
  public string? PhoneNumber { get; init; }

  /// <summary>
  /// Le rôle de l'utilisateur dans l'application.
  /// </summary>
  public Role Role { get; init; }

  /// <summary>
  /// Le genre de l'utilisateur.
  /// </summary>
  public Genre Gender { get; init; }

  /// <summary>
  /// La date de naissance de l'utilisateur.
  /// </summary>
  public DateTime Birthday { get; init; }

  /// <summary>
  /// Indique si la cotisation de l'utilisateur est à jour.
  /// </summary>
  public bool IsMembershipUpToDate { get; init; }

  /// <summary>
  /// La date du dernier paiement de cotisation enregistré.
  /// </summary>
  public DateTime? LastPayementDate { get; init; }

  /// <summary>
  /// La date et l'heure de création du compte utilisateur.
  /// </summary>
  public DateTime CreatedAt { get; init; }

  /// <summary>
  /// La date et l'heure de la dernière mise à jour des informations de l'utilisateur.
  /// </summary>
  public DateTime? UpdatedAt { get; init; }
}