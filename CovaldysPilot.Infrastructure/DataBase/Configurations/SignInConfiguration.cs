using CovaldysPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CovaldysPilot.Infrastructure.DataBase.Configurations;

public class SignInConfiguration : IEntityTypeConfiguration<SignIn>
{
  public void Configure(EntityTypeBuilder<SignIn> builder)
  {
    builder.HasKey(s => s.Id);

    builder.Property(s => s.Id)
      .ValueGeneratedOnAdd();

    builder.Property(s => s.RegistrationDate)
      .IsRequired();

    builder.Property(s => s.CreatedAt)
      .IsRequired()
      .ValueGeneratedOnAdd();

    // Relation avec User
    builder.HasOne(s => s.User)
      .WithMany(u => u.SignIns)
      .HasForeignKey(s => s.UserId)
      .OnDelete(DeleteBehavior.Cascade);

    // Relation avec Event
    builder.HasOne(s => s.Event)
      .WithMany(e => e.SignIns)
      .HasForeignKey(s => s.EventId)
      .OnDelete(DeleteBehavior.Restrict);
  }
}