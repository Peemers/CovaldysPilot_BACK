using CovaldysPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CovaldysPilot.Infrastructure.DataBase.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
  public void Configure(EntityTypeBuilder<Review> builder)
  {
    builder.HasKey(r => r.Id);

    builder.Property(r => r.Id)
      .ValueGeneratedOnAdd();

    builder.Property(r => r.Note)
      .IsRequired();

    builder.Property(r => r.CreatedAt)
      .IsRequired()
      .ValueGeneratedOnAdd();

    // Relation avec User
    builder.HasOne(r => r.User)
      .WithMany(u => u.Reviews)
      .HasForeignKey(r => r.UserId)
      .OnDelete(DeleteBehavior.Cascade);

    // Relation avec Event
    builder.HasOne(r => r.Event)
      .WithMany(e => e.Reviews)
      .HasForeignKey(r => r.EventId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}