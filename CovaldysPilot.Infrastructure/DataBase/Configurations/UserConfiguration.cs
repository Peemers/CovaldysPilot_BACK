using CovaldysPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CovaldysPilot.Infrastructure.DataBase.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.HasKey(u => u.Id);

    builder.Property(u => u.Id)
      .ValueGeneratedOnAdd();

    builder.Property(u => u.Email)
      .IsRequired()
      .HasMaxLength(256);

    builder.HasIndex(u => u.Email)
      .IsUnique(); //db : email unique

    builder.Property(u => u.Pseudo)
      .IsRequired()
      .HasMaxLength(50);

    builder.HasIndex(u => u.Pseudo)
      .IsUnique(); //db pseudo unique

    builder.Property(u => u.PasswordHash)
      .IsRequired();

    builder.Property(u => u.CreatedAt)
      .IsRequired()
      .ValueGeneratedOnAdd();
  }
}