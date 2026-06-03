using CovaldysPilot.Application.DTOs.User.Response;
using CovaldysPilot.Application.Helpers;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Mappers;
using CovaldysPilot.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CovaldysPilot.Application.Services;

public class UserService(
  IUserRepository userRepository,
  ILogger<UserService> logger) : IUserService
{
  public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
  {
    logger.LogInformation("Récupération de tous les membres");
    IEnumerable<Domain.Entities.User> users = await userRepository.GetAllAsync();
    return users.Select(u => u.ToUserResponseDto());
  }

  public async Task<UserResponseDto?> GetByIdAsync(Guid id)
  {
    logger.LogInformation("Récupération du membre : {Id}", id);
    User? user = await userRepository.GetByIdAsync(id);
    return user?.ToUserResponseDto();
  }

  public async Task DeleteAsync(Guid id)
  {
    logger.LogInformation("Suppression du membre : {Id}", id);
    User? user = await userRepository.GetByIdAsync(id);

    if (user is null)
      throw new KeyNotFoundException($"Membre avec l'id {id} introuvable.");

    await userRepository.DeleteAsync(id);
    await userRepository.SaveChangesAsync();
    logger.LogInformation("Membre supprimé : {Id}", id);
  }

  public async Task<byte[]> ExportMembersAsync(string? filter = null)
  {
    logger.LogInformation("Export des membres - Filtre: {Filter}", filter ?? "all");

    IEnumerable<Domain.Entities.User> users = await userRepository.GetAllAsync();

    users = filter switch
    {
      "effectif" => users.Where(u => u.IsMembershipUpToDate),
      "normal" => users.Where(u => !u.IsMembershipUpToDate),
      _ => users
    };

    return ExcelHelper.GenerateMembersExcel(users);
  }
}