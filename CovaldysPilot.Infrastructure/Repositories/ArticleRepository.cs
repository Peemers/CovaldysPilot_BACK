using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace CovaldysPilot.Infrastructure.Repositories;

public class ArticleRepository(CovaldysPilotDbContext context) : IArticleRepository
{
  public async Task<Article?> GetByIdAsync(Guid id)
    => await context.Articles.FindAsync(id);

  public async Task<IEnumerable<Article>> GetAllAsync()
    => await context.Articles.ToListAsync();

  public async Task<IEnumerable<Article>> GetAllArticlesWhitImageAsync()
    => await context.Articles
      .Include(a => a.Images)
      .Include(a => a.User)
      .OrderByDescending(a => a.PublicationDate)
      .ToListAsync();

  public async Task<Article?> GetByIdWithImageAsync(Guid id)
    => await context.Articles
      .Include(a => a.Images)
      .Include(a => a.User)
      .FirstOrDefaultAsync(a => a.Id == id);

  public async Task AddAsync(Article article)
    => await context.Articles.AddAsync(article);

  public Task UpdateAsync(Article article)
  {
    context.Articles.Update(article);
    return Task.CompletedTask;
  }
  public Task UpdateArticleFieldsAsync(Article article)
  {
    context.Entry(article).Property(a => a.Title).IsModified = true;
    context.Entry(article).Property(a => a.Content).IsModified = true;
    context.Entry(article).Property(a => a.Author).IsModified = true;
    context.Entry(article).Property(a => a.UpdatedAt).IsModified = true;
    context.Entry(article).Property(a => a.ViewCount).IsModified = true;
    
    foreach (var image in article.Images)
    {
      context.Entry(image).State = EntityState.Detached;
    }
    return Task.CompletedTask;
  }
  public async Task DeleteAsync(Guid id)
  {
    Article? article = await GetByIdAsync(id);
    if (article != null)
      context.Articles.Remove(article);
  }

  public async Task SaveChangesAsync()
    => await context.SaveChangesAsync();
  
  public async Task AddImageAsync(ArticleImage image)
    => await context.ArticleImages.AddAsync(image);
}