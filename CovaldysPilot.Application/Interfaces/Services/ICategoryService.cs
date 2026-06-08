using CovaldysPilot.Application.DTOs.Category.Request;
using CovaldysPilot.Application.DTOs.Category.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface ICategoryService
{
  Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
  Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto dto);
  Task DeleteAsync(Guid id);
}