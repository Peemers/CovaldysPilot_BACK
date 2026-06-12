using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Auth.Request;

/// <summary>
/// Données requises pour le changement de mot de passe.
/// </summary>
public class ChangePasswordRequestDto
{
  /// <summary>
  /// Le mot de passe actuel.
  /// </summary>
  [Required(ErrorMessage = "Le mot de passe actuel est obligatoire.")]
  public required string CurrentPassword { get; set; }

  /// <summary>
  /// Le nouveau mot de passe.
  /// </summary>
  [Required(ErrorMessage = "Le nouveau mot de passe est obligatoire.")]
  [MinLength(8, ErrorMessage = "Le nouveau mot de passe doit contenir au moins 8 caractères.")]
  [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$",
    ErrorMessage = "Le nouveau mot de passe doit contenir au moins 1 majuscule et 1 chiffre.")]
  public required string NewPassword { get; set; }

  /// <summary>
  /// La confirmation du nouveau mot de passe.
  /// </summary>
  [Required(ErrorMessage = "La confirmation du nouveau mot de passe est obligatoire.")]
  [Compare("NewPassword", ErrorMessage = "Les deux mots de passe ne correspondent pas.")]
  public required string ConfirmNewPassword { get; set; }
}