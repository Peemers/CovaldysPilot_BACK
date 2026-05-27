using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Event.Request;

public class CreateEventRequestDto
{
  [Required(ErrorMessage = "Le nom est obligatoire.")]
  [StringLength(200, MinimumLength = 3)]
  public required string Name { get; set; }

  [Required(ErrorMessage = "La description est obligatoire.")]
  public required string Description { get; set; }

  public string? Location { get; set; }
  public string? CoverImage { get; set; }

  [Required(ErrorMessage = "La date de début est obligatoire.")]
  public required DateTime StartDate { get; set; }

  [Required(ErrorMessage = "La date de fin est obligatoire.")]
  public required DateTime EndDate { get; set; }

  [Required(ErrorMessage = "La date limite d'inscription est obligatoire.")]
  public required DateTime RegistrationDeadline { get; set; }

  [Range(1, 200)]
  public int MinParticipants { get; set; } = 1;

  [Range(1, 200)]
  public int MaxParticipants { get; set; } = 1;

  public bool IsWaitingListActive { get; set; } = false;

  public List<Guid> CategoryIds { get; set; } = new List<Guid>();
}