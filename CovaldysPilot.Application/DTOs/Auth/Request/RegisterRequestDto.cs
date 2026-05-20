using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Auth.Request;

public class RegisterRequestDto
{
  [Required(ErrorMessage = "Le pseudo est obligatoire.")]
  [StringLength(50, MinimumLength = 3, ErrorMessage = "Le pseudo doit contenir entre 3 et 50 caractères.")]
  public required string Pseudo { get; set; }
  
  [Required(ErrorMessage = "L'email est obligatoire.")]
  [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
  public required string Email { get; set; }
  
  [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
  [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
  [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$",
    ErrorMessage = "Le mot de passe doit contenir au moins 1 majuscule et 1 chiffre.")]
  public required string Password { get; set; }
  
  [Required(ErrorMessage = "La confirmation du mot de passe est obligatoire.")]
  public required string ConfirmPassword { get; set; }
  
  [Required(ErrorMessage = "La date de naissance est obligatoire.")]
  public required DateTime Birthday { get; set; }
  
  public string? Gender { get; set; }
}