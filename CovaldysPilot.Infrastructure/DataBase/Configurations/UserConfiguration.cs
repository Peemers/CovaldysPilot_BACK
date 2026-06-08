using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;
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
    
    builder.Property(u => u.FirstName)
      .IsRequired()
      .HasMaxLength(100);

    builder.Property(u => u.LastName)
      .HasMaxLength(100);

    builder.Property(u => u.PhoneNumber)
      .HasMaxLength(20);
    
    builder.HasData(new User
    {
      Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
      Pseudo = "Admin",
      Email = "admin@covaldys.be",
      FirstName = "Anabelle",
      LastName = "AdminTest",
      PhoneNumber = "+32470.32.32.32",
      PasswordHash = "$2a$12$tQq2hT.BkM7p6.kvoEh52erLK6mSLS4JJlvizh251NcG37qwUrY5u",
      Role = Role.Admin,
      Birthday = new DateTime(1980, 1, 1),
      IsMembershipUpToDate = true,
      CreatedAt = new DateTime(2026, 1, 1)
    });
  }
}