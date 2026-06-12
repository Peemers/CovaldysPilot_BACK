using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Auth.Request;

/// <summary>
/// Données requises pour la connexion d'un utilisateur.
/// </summary>
public class LoginRequestDto
{
  /// <summary>
  /// L'adresse e-mail ou le pseudonyme.
  /// </summary>
  [Required(ErrorMessage = "L'email ou le pseudo est obligatoire.")]
  [MaxLength(256, ErrorMessage = "L'email ou le pseudo ne peut pas dépasser 256 caractères.")]
  public required string EmailOrPseudo { get; set; }
  
  /// <summary>
  /// Le mot de passe.
  /// </summary>
  [Required(ErrorMessage = "Le mot de passe est obligatoire.")] 
  public required string Password { get; set; }
  
  //pas de contrainte de taille au cas ou un user avec un ancien mdp valide voudrait se connecter.
}