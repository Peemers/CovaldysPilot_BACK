using System.ComponentModel.DataAnnotations;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.DTOs.User.Request;

/// <summary>
/// Données de requête pour la création manuelle d'un utilisateur par un administrateur.
/// </summary>
public class CreateUserManuallyRequestDto
{
  /// <summary>
  /// Le prénom de l'utilisateur.
  /// </summary>
  [Required]
  [MaxLength(100)]
  public required string FirstName { get; set; }

  /// <summary>
  /// Le nom de famille de l'utilisateur.
  /// </summary>
  [Required]
  [MaxLength(100)]
  public required string LastName { get; set; }

  /// <summary>
  /// L'adresse e-mail de l'utilisateur.
  /// </summary>
  [Required]
  [EmailAddress]
  [MaxLength(256)]
  public required string Email { get; set; }

  /// <summary>
  /// Le pseudonyme unique de l'utilisateur.
  /// </summary>
  [Required]
  [MaxLength(50)]
  public required string Pseudo { get; set; }

  /// <summary>
  /// La date de naissance de l'utilisateur.
  /// </summary>
  [Required]
  public required DateTime Birthday { get; set; }

  /// <summary>
  /// Le genre de l'utilisateur.
  /// </summary>
  [Required]
  public required Genre Gender { get; set; }

  /// <summary>
  /// Indique si la cotisation de l'utilisateur est à jour.
  /// </summary>
  public bool IsMembershipUpToDate { get; set; } = false;
}