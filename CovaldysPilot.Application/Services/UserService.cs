using CovaldysPilot.Application.DTOs.User.Request;
using CovaldysPilot.Application.DTOs.User.Response;
using CovaldysPilot.Application.Email.Templates;
using CovaldysPilot.Application.Helpers;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Mappers;
using CovaldysPilot.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CovaldysPilot.Application.Services;

public class UserService(
  IUserRepository userRepository,
  IEmailService emailService,
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

  public async Task<CreateUserManuallyResponseDto> AddManuallyAsync(CreateUserManuallyRequestDto dto)
  {
    logger.LogInformation("Ajout manuel d'un membre : {Email}", dto.Email);

    bool emailExists = await userRepository.EmailExistsAsync(dto.Email);
    if (emailExists)
      throw new InvalidOperationException($"L'email {dto.Email} est déjà utilisé.");

    bool pseudoExists = await userRepository.PseudoExistsAsync(dto.Pseudo);
    if (pseudoExists)
      throw new InvalidOperationException($"Le pseudo {dto.Pseudo} est déjà utilisé.");

    // On garde le mot de passe en clair pour l'envoyer par email et le retourner
    string tempPassword = PasswordHelper.GenerateRandomPassword();
    string passwordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword);

    User user = dto.ToUserFromManualCreation(passwordHash);

    await userRepository.AddAsync(user);
    await userRepository.SaveChangesAsync();

    // Envoi email au membre avec son mot de passe temporaire
    await emailService.SendEmail(
      user.Email,
      user.FirstName,
      "Bienvenue sur Covaldys — Votre compte a été créé",
      EmailTemplates.ManualAccountCreation(user.FirstName, user.Email, tempPassword)
    );

    logger.LogInformation("Membre ajouté manuellement : {Id}", user.Id);
    
    // On retourne le nouveau DTO avec le mot de passe temporaire en clair
    return user.ToCreateUserManuallyResponseDto(tempPassword);
  }
}