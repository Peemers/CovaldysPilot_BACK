using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Auth.Request;

public class LoginRequestDto
{
  [Required(ErrorMessage = "L'email ou le pseudo est obligatoire.")]
  public required string EmailOrPseudo { get; set; }
  
  [Required(ErrorMessage = "Le mot de passe est obligatoire.")] 
  public required string Password { get; set; }
  
  //pas de contrainte de taille au cas ou un user avec un ancien mdp valide voudrait se connecter.
}