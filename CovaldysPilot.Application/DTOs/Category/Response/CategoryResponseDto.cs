namespace CovaldysPilot.Application.DTOs.Category.Response;

public class CategoryResponseDto
{
  public Guid Id { get; init; }
  public required string Name { get; init; }
  public DateTime CreatedAt { get; init; }
}