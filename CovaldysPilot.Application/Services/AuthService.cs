using CovaldysPilot.Application.DTOs.Auth.Request;
using CovaldysPilot.Application.DTOs.Auth.Response;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Mappers;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;

namespace CovaldysPilot.Application.Services;

public class AuthService(
  IUserRepository userRepository,
  IRefreshTokenRepository refreshTokenRepository,
  IJwtService jwtService) : IAuthService
{
  public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
  {
    if (await userRepository.EmailExistsAsync(dto.Email))
      throw new InvalidOperationException("Cet email est déjà utilisé.");

    if (await userRepository.PseudoExistsAsync(dto.Pseudo))
      throw new InvalidOperationException("Ce pseudo est déjà utilisé.");

    if (dto.Password != dto.ConfirmPassword)
      throw new InvalidOperationException("Les mots de passe ne correspondent pas.");

    string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
    User user = dto.ToUser(passwordHash);

    await userRepository.AddAsync(user);
    await userRepository.SaveChangesAsync();

    return await GenerateAuthResponse(user);
  }

  public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
  {
    User user = await userRepository.GetByEmailOrPseudoAsync(dto.EmailOrPseudo)
                ?? throw new InvalidOperationException("Email/pseudo ou mot de passe incorrect.");

    if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
      throw new InvalidOperationException("Email/pseudo ou mot de passe incorrect.");

    return await GenerateAuthResponse(user);
  }

  public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
  {
    RefreshToken refreshToken = await refreshTokenRepository.GetByTokenAsync(dto.RefreshToken)
                                ?? throw new InvalidOperationException("Refresh token invalide.");

    if (refreshToken.RevokedAt != null)
      throw new InvalidOperationException("Refresh token révoqué.");

    if (refreshToken.ExpirationDate < DateTime.UtcNow)
      throw new InvalidOperationException("Refresh token expiré.");

    await refreshTokenRepository.RevokeTokenAsync(dto.RefreshToken);
    await refreshTokenRepository.SaveChangesAsync();

    return await GenerateAuthResponse(refreshToken.User);
  }

  public async Task RevokeTokenAsync(string refreshToken)
  {
    await refreshTokenRepository.RevokeTokenAsync(refreshToken);
    await refreshTokenRepository.SaveChangesAsync();
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