using CovaldysPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CovaldysPilot.Infrastructure.DataBase.Configurations;

public class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
{
  public void Configure(EntityTypeBuilder<EventCategory> builder)
  {
    // Clé primaire composite
    builder.HasKey(ec => new { ec.EventId, ec.CategoryId });

    // Relation avec Event
    builder.HasOne(ec => ec.Event)
      .WithMany(e => e.EventCategories)
      .HasForeignKey(ec => ec.EventId)
      .OnDelete(DeleteBehavior.Cascade);

    // Relation avec Category
    builder.HasOne(ec => ec.Category)
      .WithMany(c => c.EventCategories)
      .HasForeignKey(ec => ec.CategoryId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}