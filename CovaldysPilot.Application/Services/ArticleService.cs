using CovaldysPilot.Application.DTOs.Article.Request;
using CovaldysPilot.Application.DTOs.Article.Response;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Mappers;
using CovaldysPilot.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CovaldysPilot.Application.Services;

public class ArticleService(
    IArticleRepository articleRepository,
    ILogger<ArticleService> logger) : IArticleService
{
    public async Task<IEnumerable<ArticleResponseDto>> GetAllAsync()
    {
        logger.LogInformation("Récupération de tous les articles");
        IEnumerable<Article> articles = await articleRepository.GetAllArticlesWhitImageAsync();
        return articles.Select(a => a.ToArticleResponseDto());
    }

    public async Task<ArticleResponseDto?> GetByIdAsync(Guid id)
    {
        logger.LogInformation("Récupération de l'article {Id}", id);
        Article? article = await articleRepository.GetByIdWithImageAsync(id);
        if (article is null) return null;

        article.ViewCount++;
        await articleRepository.UpdateAsync(article);
        await articleRepository.SaveChangesAsync();

        return article.ToArticleResponseDto();
    }

    public async Task<ArticleResponseDto> CreateAsync(CreateArticleRequestDto dto, Guid? userId)
    {
        logger.LogInformation("Création d'un article : {Title}", dto.Title);
        Article article = dto.ToArticle(userId);

        await articleRepository.AddAsync(article);
        await articleRepository.SaveChangesAsync();

        Article? created = await articleRepository.GetByIdWithImageAsync(article.Id);
        if (created is null)
            throw new InvalidOperationException("Erreur lors de la récupération de l'article créé.");

        logger.LogInformation("Article créé : {Title}", dto.Title);
        return created.ToArticleResponseDto();
    }

    public async Task<ArticleResponseDto> UpdateAsync(Guid id, UpdateArticleRequestDto dto)
    {
        logger.LogInformation("Modification de l'article {Id}", id);
        Article? article = await articleRepository.GetByIdWithImageAsync(id);

        if (article is null)
            throw new KeyNotFoundException($"Article {id} introuvable.");

        article.UpdateFromDto(dto);

        await articleRepository.UpdateAsync(article);
        await articleRepository.SaveChangesAsync();

        logger.LogInformation("Article modifié : {Id}", id);
        return article.ToArticleResponseDto();
    }

    public async Task DeleteAsync(Guid id)
    {
        logger.LogInformation("Suppression de l'article {Id}", id);
        Article? article = await articleRepository.GetByIdAsync(id);

        if (article is null)
            throw new KeyNotFoundException($"Article {id} introuvable.");

        await articleRepository.DeleteAsync(id);
        await articleRepository.SaveChangesAsync();
        logger.LogInformation("Article supprimé : {Id}", id);
    }
    public async Task<ArticleResponseDto> AddImageAsync(Guid id, string imageUrl)
    {
        logger.LogInformation("Ajout d'une image à l'article {Id}", id);
        Article? article = await articleRepository.GetByIdWithImageAsync(id);

        if (article is null)
            throw new KeyNotFoundException($"Article {id} introuvable.");

        if (article.Images.Count >= 2)
            throw new InvalidOperationException("Un article ne peut pas avoir plus de 2 images.");

        var image = new ArticleImage
        {
            Id = Guid.NewGuid(),
            ArticleId = id,
            Url = imageUrl,
            CreatedAt = DateTime.UtcNow
        };

        await articleRepository.AddImageAsync(image); // 👈 insert direct
        await articleRepository.SaveChangesAsync();

        Article? updated = await articleRepository.GetByIdWithImageAsync(id);
        logger.LogInformation("Image ajoutée à l'article {Id}", id);
        return updated!.ToArticleResponseDto();
    }

    public async Task DeleteImageAsync(Guid articleId, Guid imageId)
    {
        logger.LogInformation("Suppression de l'image {ImageId} de l'article {ArticleId}", imageId, articleId);
        Article? article = await articleRepository.GetByIdWithImageAsync(articleId);

        if (article is null)
            throw new KeyNotFoundException($"Article {articleId} introuvable.");

        ArticleImage? image = article.Images.FirstOrDefault(i => i.Id == imageId);
        if (image is null)
            throw new KeyNotFoundException($"Image {imageId} introuvable.");

        article.Images.Remove(image);

        await articleRepository.UpdateAsync(article);
        await articleRepository.SaveChangesAsync();

        logger.LogInformation("Image {ImageId} supprimée", imageId);
    }
}