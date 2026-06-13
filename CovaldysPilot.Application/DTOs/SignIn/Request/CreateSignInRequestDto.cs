using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.SignIn.Request;

/// <summary>
/// Données requises pour l'inscription d'un utilisateur à un événement.
/// </summary>
public class CreateSignInRequestDto
{
  /// <summary>
  /// L'identifiant unique de l'événement.
  /// </summary>
  [Required]
  public required Guid EventId { get; set; }
}