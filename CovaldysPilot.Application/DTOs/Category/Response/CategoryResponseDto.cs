namespace CovaldysPilot.Application.DTOs.Category.Response;

public class CategoryResponseDto
{
  public Guid Id { get; set; }
  public required string Name { get; set; }
  public DateTime CreatedAt { get; set; }
}