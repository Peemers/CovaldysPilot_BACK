using CovaldysPilot.Application.DTOs.User.Response;

namespace CovaldysPilot.Application.Interfaces.Services;

public interface IUserService
{
  Task<IEnumerable<UserResponseDto>> GetAllAsync();
  Task<UserResponseDto?> GetByIdAsync(Guid id);
  Task DeleteAsync(Guid id);
  Task<byte[]> ExportMembersAsync(string? filter = null);
}