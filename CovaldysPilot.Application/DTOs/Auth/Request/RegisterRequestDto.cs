using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Auth.Request;

/// <summary>
/// Données requises pour l'inscription d'un nouvel utilisateur.
/// </summary>
public class RegisterRequestDto
{
  /// <summary>
  /// Le pseudonyme du membre.
  /// </summary>
  [Required(ErrorMessage = "Le pseudo est obligatoire.")]
  [StringLength(50, MinimumLength = 3, ErrorMessage = "Le pseudo doit contenir entre 3 et 50 caractères.")]
  public required string Pseudo { get; set; }
  
  /// <summary>
  /// Le prénom du membre.
  /// </summary>
  [Required(ErrorMessage = "Le prénom est obligatoire.")]
  [StringLength(100, MinimumLength = 2)]
  public required string FirstName { get; set; }

  /// <summary>
  /// Le nom de famille du membre.
  /// </summary>
  [MaxLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
  public string? LastName { get; set; }

  /// <summary>
  /// Le numéro de téléphone du membre.
  /// </summary>
  [MaxLength(20, ErrorMessage = "Le numéro de téléphone ne peut pas dépasser 20 caractères.")]
  public string? PhoneNumber { get; set; }
  
  /// <summary>
  /// L'adresse e-mail du membre.
  /// </summary>
  [Required(ErrorMessage = "L'email est obligatoire.")]
  [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
  [MaxLength(256, ErrorMessage = "L'email ne peut pas dépasser 256 caractères.")]
  public required string Email { get; set; }
  
  /// <summary>
  /// Le mot de passe.
  /// </summary>
  [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
  [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
  [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$",
    ErrorMessage = "Le mot de passe doit contenir au moins 1 majuscule et 1 chiffre.")]
  public required string Password { get; set; }
  
  /// <summary>
  /// La confirmation du mot de passe.
  /// </summary>
  [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
  [Compare("Password", ErrorMessage = "La confirmation ne correspond pas au mot de passe.")]
  public required string ConfirmPassword { get; set; }
  
  /// <summary>
  /// La date de naissance.
  /// </summary>
  [Required(ErrorMessage = "La date de naissance est obligatoire.")]
  public required DateTime Birthday { get; set; }
  
  /// <summary>
  /// Le genre (optionnel).
  /// </summary>
  public string? Gender { get; set; }
}