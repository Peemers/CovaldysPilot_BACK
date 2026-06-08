using System.ComponentModel.DataAnnotations;

namespace CovaldysPilot.Application.DTOs.Category.Request;

public class CreateCategoryRequestDto
{
  [Required]
  [StringLength(100, MinimumLength = 2,  ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères.")]
  public required string Name { get; set; }
}