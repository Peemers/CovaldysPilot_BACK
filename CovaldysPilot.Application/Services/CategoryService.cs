using CovaldysPilot.Application.DTOs.Category.Request;
using CovaldysPilot.Application.DTOs.Category.Response;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Mappers;
using Microsoft.Extensions.Logging;

namespace CovaldysPilot.Application.Services;

public class CategoryService(
  ICategoryRepository categoryRepository,
  ILogger<CategoryService> logger) : ICategoryService 
{
  public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
  {
    logger.LogInformation("Récupération de toutes les catégories");
    IEnumerable<Domain.Entities.Category> categories = await categoryRepository.GetAllAsync();
    return categories.Select(c => c.ToCategoryResponseDto());
  }

  public async Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto dto)
  {
    logger.LogInformation("Création d'une catégorie : {Name}", dto.Name);
    if (await categoryRepository.NameExistsAsync(dto.Name))
      throw new InvalidOperationException($"La catégorie '{dto.Name}' existe déjà");

    Domain.Entities.Category category = dto.ToCategory(); //conflit namespace
    
    await categoryRepository.AddAsync(category);
    await categoryRepository.SaveChangesAsync();
    
    logger.LogInformation("Catégorie créée : {Name}", dto.Name);
    return category.ToCategoryResponseDto();
  }

  public async Task DeleteAsync(Guid id)
  {
    logger.LogInformation("Suppression de la catégorie : {Id}", id);
    await categoryRepository.DeleteAsync(id);
    await categoryRepository.SaveChangesAsync();
    logger.LogInformation("Catégorie supprimée : {Id}", id);
  }
}