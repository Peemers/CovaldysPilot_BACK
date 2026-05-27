using CovaldysPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CovaldysPilot.Infrastructure.DataBase.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
  public void Configure(EntityTypeBuilder<Event> builder)
  {
    builder.HasKey(e => e.Id);

    builder.Property(e => e.Id)
      .ValueGeneratedOnAdd();

    builder.Property(e => e.Name)
      .IsRequired()
      .HasMaxLength(200);

    builder.Property(e => e.Description)
      .IsRequired();

    builder.Property(e => e.Location)
      .HasMaxLength(300);

    builder.Property(e => e.CoverImage)
      .HasMaxLength(500);

    builder.Property(e => e.StartDate)
      .IsRequired();

    builder.Property(e => e.EndDate)
      .IsRequired();

    builder.Property(e => e.RegistrationDeadline)
      .IsRequired();

    builder.Property(e => e.Status)
      .IsRequired()
      .HasConversion<string>();

    builder.Property(e => e.CreatedAt)
      .IsRequired()
      .ValueGeneratedOnAdd();

    builder.Property(e => e.Price)
      .HasPrecision(10, 2);
  }
}