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
  #region GetAllAsync
  /// <inheritdoc/>
  public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
  {
    logger.LogInformation("Récupération de tous les membres");
    IEnumerable<Domain.Entities.User> users = await userRepository.GetAllAsync();
    return users.Select(u => u.ToUserResponseDto());
  }
  #endregion

  #region GetByIdAsync
  /// <inheritdoc/>
  public async Task<UserResponseDto?> GetByIdAsync(Guid id)
  {
    logger.LogInformation("Récupération du membre : {Id}", id);
    User? user = await userRepository.GetByIdAsync(id);
    return user?.ToUserResponseDto();
  }
  #endregion

  #region DeleteAsync
  /// <inheritdoc/>
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
  #endregion

  #region ExportMembersAsync
  /// <inheritdoc/>
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
  #endregion

  #region AddManuallyAsync
  /// <inheritdoc/>
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
  #endregion
  
  #region PayCotisationAsync
  /// <inheritdoc/>
  public async Task PayCotisationAsync(Guid userId)
  {
    logger.LogInformation("Paiement de la cotisation pour : {UserId}", userId);

    User? user = await userRepository.GetByIdAsync(userId);
    if (user is null)
      throw new KeyNotFoundException($"Membre {userId} introuvable.");

    if (user.IsMembershipUpToDate)
      throw new InvalidOperationException("Votre cotisation est déjà à jour.");

    user.IsMembershipUpToDate = true;
    user.LastPayementDate = DateTime.Now;
    user.UpdatedAt = DateTime.Now;

    await userRepository.UpdateAsync(user);
    await userRepository.SaveChangesAsync();

    logger.LogInformation("Cotisation payée pour : {UserId}", userId);
  }
  #endregion
}