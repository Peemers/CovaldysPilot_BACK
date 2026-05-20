using CovaldysPilot.Application.DTOs.Auth.Request;
using CovaldysPilot.Application.DTOs.Auth.Response;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Mappers;
using CovaldysPilot.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace CovaldysPilot.Application.Services;

public class AuthService(
  IUserRepository userRepository,
  IRefreshTokenRepository refreshTokenRepository,
  IJwtService jwtService,
  ILogger<AuthService> logger) : IAuthService
{
  public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
  {
    logger.LogInformation("Tentative d'inscription pour l'email : {Email}", dto.Email);

    if (await userRepository.EmailExistsAsync(dto.Email))
    {
      logger.LogWarning("Email déjà utilisé : {Email}", dto.Email);
      throw new InvalidOperationException("Cet email est déjà utilisé.");
    }


    if (await userRepository.PseudoExistsAsync(dto.Pseudo))
    {
      logger.LogWarning("Pseudo déjà utilisé : {Pseudo}", dto.Pseudo);
      throw new InvalidOperationException("Ce pseudo est déjà utilisé.");
    }
      

    if (dto.Password != dto.ConfirmPassword)
    {
      logger.LogWarning("Mots de passe non conformes pour : {Email}", dto.Email);
      throw new InvalidOperationException("Les mots de passe ne correspondent pas.");
    }

    string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
    User user = dto.ToUser(passwordHash);

    await userRepository.AddAsync(user);
    await userRepository.SaveChangesAsync();
    
    logger.LogInformation("Inscription réussie pour : {Pseudo}", dto.Pseudo);
    return await GenerateAuthResponse(user);
  }

  public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
  {
    logger.LogInformation("Tentative de connexion pour : {EmailOrPseudo}", dto.EmailOrPseudo);
    
    User user = await userRepository.GetByEmailOrPseudoAsync(dto.EmailOrPseudo)
                ?? throw new InvalidOperationException("Email/pseudo ou mot de passe incorrect.");

    if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
    {
      logger.LogWarning("Mot de passe incorrect pour : {EmailOrPseudo}", dto.EmailOrPseudo);
      throw new InvalidOperationException("Email/pseudo ou mot de passe incorrect.");
    }
    
    logger.LogInformation("Connexion réussie pour : {Pseudo}", user.Pseudo);
    return await GenerateAuthResponse(user);
  }

  public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
  {
    logger.LogInformation("Tentative de refresh token");
    
    RefreshToken refreshToken = await refreshTokenRepository.GetByTokenAsync(dto.RefreshToken)
                                ?? throw new InvalidOperationException("Refresh token invalide.");

    if (refreshToken.RevokedAt != null)
    {
      logger.LogWarning("Refresh token déjà révoqué");
      throw new InvalidOperationException("Refresh token révoqué.");
    }

    if (refreshToken.ExpirationDate < DateTime.UtcNow)
    {
      logger.LogWarning("Refresh token expiré");
      throw new InvalidOperationException("Refresh token expiré.");
    }

    await refreshTokenRepository.RevokeTokenAsync(dto.RefreshToken);
    await refreshTokenRepository.SaveChangesAsync();
    
    logger.LogInformation("Refresh token réussi pour : {Pseudo}", refreshToken.User.Pseudo);
    return await GenerateAuthResponse(refreshToken.User);
  }

  public async Task RevokeTokenAsync(string refreshToken)
  {
    logger.LogInformation("Révocation du refresh token");
    await refreshTokenRepository.RevokeTokenAsync(refreshToken);
    await refreshTokenRepository.SaveChangesAsync();
    logger.LogInformation("Refresh token révoqué avec succès");
  }

  private async Task<AuthResponseDto> GenerateAuthResponse(User user)
  {
    string accessToken = jwtService.GenerateAccessToken(user);
    string newRefreshToken = jwtService.GenerateRefreshToken();
    DateTime expiryDate = jwtService.GetRefreshTokenExpiryDate();

    RefreshToken refreshTokenEntity = user.ToRefreshTokenEntity(newRefreshToken, expiryDate);

    await refreshTokenRepository.AddAsync(refreshTokenEntity);
    await refreshTokenRepository.SaveChangesAsync();

    return user.ToAuthResponseDto(accessToken, newRefreshToken, expiryDate);
  }
}