using CovaldysPilot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CovaldysPilot.Infrastructure.DataBase.Context;

public class CovaldysPilotDbContext(DbContextOptions<CovaldysPilotDbContext> options) : DbContext(options)
{
  //syntaxe plus récente, plus sécuritaire car pas de setter, impossible d'écraser par erreur.
  public DbSet<User> Users => Set<User>();
  public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
  public DbSet<Event> Events => Set<Event>();
  public DbSet<Category> Categories => Set<Category>();
  public DbSet<EventCategory> EventCategories => Set<EventCategory>();
  public DbSet<SignIn> SignIns => Set<SignIn>();
  public DbSet<Review> Reviews => Set<Review>();
  public DbSet<Article> Articles => Set<Article>();
  public DbSet<ArticleImage> ArticleImages => Set<ArticleImage>();
  public DbSet<SiteConfiguration> SiteConfigurations => Set<SiteConfiguration>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Applique toutes les configurations du dossier Configurations
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(CovaldysPilotDbContext).Assembly);
  }
}