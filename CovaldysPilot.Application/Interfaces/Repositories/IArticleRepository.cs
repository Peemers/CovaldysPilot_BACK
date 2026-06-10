using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Interfaces.Repositories;

public interface IArticleRepository : IBaseRepository<Article>
{
  Task<IEnumerable<Article>> GetAllArticlesWhitImageAsync();
  Task<Article?> GetByIdWithImageAsync(Guid id);
  Task AddImageAsync(ArticleImage image);
}