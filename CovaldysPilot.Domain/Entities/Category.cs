namespace CovaldysPilot.Domain.Entities;

public class Category : BaseEntity
{
  public required string Name { get; set; }
  public ICollection<Event> Events { get; set; } = new List<Event>();
}