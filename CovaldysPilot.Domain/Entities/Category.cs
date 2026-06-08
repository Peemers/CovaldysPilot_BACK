namespace CovaldysPilot.Domain.Entities;

public class Category : BaseEntity
{
  public required string Name { get; set; }
  public ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();
}