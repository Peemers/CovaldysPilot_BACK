using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Auth.Request;

/// <summary>
/// Données requises pour le rafraîchissement d'un jeton d'accès.
/// </summary>
public class RefreshTokenRequestDto
{
  /// <summary>
  /// Le jeton de rafraîchissement.
  /// </summary>
  [Required(ErrorMessage = "Le refresh token est obligatoire.")]
  public required string RefreshToken { get; set; }
}