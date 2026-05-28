using CovaldysPilot.Application.DTOs.Category.Request;
using CovaldysPilot.Application.DTOs.Category.Response;
using CovaldysPilot.Domain.Entities;

namespace CovaldysPilot.Application.Mappers;

public static class CategoryMapper
{
  public static CategoryResponseDto ToCategoryResponseDto(this Category category)
  {
    return new CategoryResponseDto
    {
      Id = category.Id,
      Name = category.Name,
      CreatedAt = category.CreatedAt,
    };
  }

  public static Category ToCategory(this CreateCategoryRequestDto category)
  {
    return new Category
    {
      Name = category.Name,
      CreatedAt = DateTime.UtcNow
    };
  }
}