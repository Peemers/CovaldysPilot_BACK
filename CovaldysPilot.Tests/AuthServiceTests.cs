using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CovaldysPilot.Application.DTOs.Auth.Request;
using CovaldysPilot.Application.DTOs.Auth.Response;
using CovaldysPilot.Application.Interfaces.Repositories;
using CovaldysPilot.Application.Interfaces.Services;
using CovaldysPilot.Application.Services;
using CovaldysPilot.Domain.Entities;
using CovaldysPilot.Domain.Enums;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace CovaldysPilot.Tests;

public class AuthServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
        _jwtService = Substitute.For<IJwtService>();
        _logger = Substitute.For<ILogger<AuthService>>();
        
        _authService = new AuthService(
            _userRepository,
            _refreshTokenRepository,
            _jwtService,
            _logger
        );
    }

    #region RegisterAsync Tests

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Email = "exists@test.com",
            Pseudo = "NewUser",
            FirstName = "First",
            LastName = "Last",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            Birthday = DateTime.UtcNow.AddYears(-20)
        };

        _userRepository.EmailExistsAsync(request.Email).Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(request));
        Assert.Equal("Cet email est déjà utilisé.", exception.Message);
        
        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
        await _userRepository.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task RegisterAsync_WhenPseudoAlreadyExists_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Email = "new@test.com",
            Pseudo = "ExistingUser",
            FirstName = "First",
            LastName = "Last",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            Birthday = DateTime.UtcNow.AddYears(-20)
        };

        _userRepository.EmailExistsAsync(request.Email).Returns(false);
        _userRepository.PseudoExistsAsync(request.Pseudo).Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(request));
        Assert.Equal("Ce pseudo est déjà utilisé.", exception.Message);

        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
        await _userRepository.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task RegisterAsync_WhenPasswordsDoNotMatch_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Email = "new@test.com",
            Pseudo = "NewUser",
            FirstName = "First",
            LastName = "Last",
            Password = "Password123!",
            ConfirmPassword = "DifferentPassword123!",
            Birthday = DateTime.UtcNow.AddYears(-20)
        };

        _userRepository.EmailExistsAsync(request.Email).Returns(false);
        _userRepository.PseudoExistsAsync(request.Pseudo).Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(request));
        Assert.Equal("Les mots de passe ne correspondent pas.", exception.Message);

        await _userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
        await _userRepository.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task RegisterAsync_WhenRequestIsValid_RegistersUserAndReturnsAuthResponse()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            Email = "new@test.com",
            Pseudo = "NewUser",
            FirstName = "First",
            LastName = "Last",
            Password = "Password123!",
            ConfirmPassword = "Password123!",
            Birthday = DateTime.UtcNow.AddYears(-20)
        };

        _userRepository.EmailExistsAsync(request.Email).Returns(false);
        _userRepository.PseudoExistsAsync(request.Pseudo).Returns(false);

        var expectedAccessToken = "accessToken123";
        var expectedRefreshToken = "refreshToken123";
        var expectedExpiry = DateTime.UtcNow.AddDays(7);

        _jwtService.GenerateAccessToken(Arg.Any<User>()).Returns(expectedAccessToken);
        _jwtService.GenerateRefreshToken().Returns(expectedRefreshToken);
        _jwtService.GetRefreshTokenExpiryDate().Returns(expectedExpiry);

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Pseudo, result.Pseudo);
        Assert.Equal(expectedAccessToken, result.AccessToken);
        Assert.Equal(expectedRefreshToken, result.RefreshToken);
        Assert.Equal(expectedExpiry, result.ExpiresAt);

        await _userRepository.Received(1).AddAsync(Arg.Is<User>(u => 
            u.Email == request.Email && 
            u.Pseudo == request.Pseudo && 
            u.FirstName == request.FirstName &&
            u.LastName == request.LastName &&
            BCrypt.Net.BCrypt.Verify(request.Password, u.PasswordHash)
        ));
        await _userRepository.Received(1).SaveChangesAsync();
        
        await _refreshTokenRepository.Received(1).AddAsync(Arg.Is<RefreshToken>(rt =>
            rt.Token == expectedRefreshToken &&
            rt.ExpirationDate == expectedExpiry
        ));
        await _refreshTokenRepository.Received(1).SaveChangesAsync();
    }

    #endregion

    #region LoginAsync Tests

    [Fact]
    public async Task LoginAsync_WhenUserNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            EmailOrPseudo = "nonexistent@test.com",
            Password = "Password123!"
        };

        _userRepository.GetByEmailOrPseudoAsync(request.EmailOrPseudo).Returns((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.LoginAsync(request));
        Assert.Equal("Email/pseudo ou mot de passe incorrect.", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsIncorrect_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            EmailOrPseudo = "user@test.com",
            Password = "WrongPassword!"
        };

        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Pseudo = "UserPseudo",
            Email = "user@test.com",
            FirstName = "First",
            LastName = "Last",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword!"),
            Birthday = DateTime.UtcNow.AddYears(-20),
            Role = Role.Membre
        };

        _userRepository.GetByEmailOrPseudoAsync(request.EmailOrPseudo).Returns(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.LoginAsync(request));
        Assert.Equal("Email/pseudo ou mot de passe incorrect.", exception.Message);
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreValid_ReturnsAuthResponse()
    {
        // Arrange
        var request = new LoginRequestDto
        {
            EmailOrPseudo = "user@test.com",
            Password = "CorrectPassword!"
        };

        var existingUser = new User
        {
            Id = Guid.NewGuid(),
            Pseudo = "UserPseudo",
            Email = "user@test.com",
            FirstName = "First",
            LastName = "Last",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword!"),
            Birthday = DateTime.UtcNow.AddYears(-20),
            Role = Role.Membre
        };

        _userRepository.GetByEmailOrPseudoAsync(request.EmailOrPseudo).Returns(existingUser);

        var expectedAccessToken = "accessToken123";
        var expectedRefreshToken = "refreshToken123";
        var expectedExpiry = DateTime.UtcNow.AddDays(7);

        _jwtService.GenerateAccessToken(existingUser).Returns(expectedAccessToken);
        _jwtService.GenerateRefreshToken().Returns(expectedRefreshToken);
        _jwtService.GetRefreshTokenExpiryDate().Returns(expectedExpiry);

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(existingUser.Pseudo, result.Pseudo);
        Assert.Equal(expectedAccessToken, result.AccessToken);
        Assert.Equal(expectedRefreshToken, result.RefreshToken);
        Assert.Equal(expectedExpiry, result.ExpiresAt);

        await _refreshTokenRepository.Received(1).AddAsync(Arg.Is<RefreshToken>(rt =>
            rt.Token == expectedRefreshToken &&
            rt.UserId == existingUser.Id &&
            rt.ExpirationDate == expectedExpiry
        ));
        await _refreshTokenRepository.Received(1).SaveChangesAsync();
    }

    #endregion

    #region RefreshTokenAsync Tests

    [Fact]
    public async Task RefreshTokenAsync_WhenTokenNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RefreshTokenRequestDto
        {
            RefreshToken = "nonexistentToken"
        };

        _refreshTokenRepository.GetByTokenAsync(request.RefreshToken).Returns((RefreshToken?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RefreshTokenAsync(request));
        Assert.Equal("Refresh token invalide.", exception.Message);
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenTokenIsAlreadyRevoked_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RefreshTokenRequestDto
        {
            RefreshToken = "revokedToken"
        };

        var refreshToken = new RefreshToken
        {
            Token = "revokedToken",
            ExpirationDate = DateTime.UtcNow.AddDays(1),
            RevokedAt = DateTime.UtcNow.AddHours(-1),
            UserId = Guid.NewGuid()
        };

        _refreshTokenRepository.GetByTokenAsync(request.RefreshToken).Returns(refreshToken);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RefreshTokenAsync(request));
        Assert.Equal("Refresh token révoqué.", exception.Message);

        await _refreshTokenRepository.DidNotReceive().RevokeTokenAsync(Arg.Any<string>());
        await _refreshTokenRepository.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenTokenIsExpired_ThrowsInvalidOperationException()
    {
        // Arrange
        var request = new RefreshTokenRequestDto
        {
            RefreshToken = "expiredToken"
        };

        var refreshToken = new RefreshToken
        {
            Token = "expiredToken",
            ExpirationDate = DateTime.UtcNow.AddMinutes(-5),
            RevokedAt = null,
            UserId = Guid.NewGuid()
        };

        _refreshTokenRepository.GetByTokenAsync(request.RefreshToken).Returns(refreshToken);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RefreshTokenAsync(request));
        Assert.Equal("Refresh token expiré.", exception.Message);

        await _refreshTokenRepository.DidNotReceive().RevokeTokenAsync(Arg.Any<string>());
        await _refreshTokenRepository.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    public async Task RefreshTokenAsync_WhenTokenIsValid_RevokesOldTokenAndReturnsNewAuthResponse()
    {
        // Arrange
        var request = new RefreshTokenRequestDto
        {
            RefreshToken = "validToken"
        };

        var user = new User
        {
            Id = Guid.NewGuid(),
            Pseudo = "UserPseudo",
            Email = "user@test.com",
            FirstName = "First",
            LastName = "Last",
            PasswordHash = "hashedPassword",
            Birthday = DateTime.UtcNow.AddYears(-20),
            Role = Role.Membre
        };

        var refreshToken = new RefreshToken
        {
            Token = "validToken",
            ExpirationDate = DateTime.UtcNow.AddDays(1),
            RevokedAt = null,
            UserId = user.Id,
            User = user
        };

        _refreshTokenRepository.GetByTokenAsync(request.RefreshToken).Returns(refreshToken);

        var expectedAccessToken = "newAccessToken";
        var expectedRefreshToken = "newRefreshToken";
        var expectedExpiry = DateTime.UtcNow.AddDays(7);

        _jwtService.GenerateAccessToken(user).Returns(expectedAccessToken);
        _jwtService.GenerateRefreshToken().Returns(expectedRefreshToken);
        _jwtService.GetRefreshTokenExpiryDate().Returns(expectedExpiry);

        // Act
        var result = await _authService.RefreshTokenAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Pseudo, result.Pseudo);
        Assert.Equal(expectedAccessToken, result.AccessToken);
        Assert.Equal(expectedRefreshToken, result.RefreshToken);

        // Verify the old token is revoked
        await _refreshTokenRepository.Received(1).RevokeTokenAsync(request.RefreshToken);
        // Verify new token is saved
        await _refreshTokenRepository.Received(1).AddAsync(Arg.Is<RefreshToken>(rt =>
            rt.Token == expectedRefreshToken &&
            rt.UserId == user.Id &&
            rt.ExpirationDate == expectedExpiry
        ));
        await _refreshTokenRepository.Received(2).SaveChangesAsync(); // Once for revocation, once for adding new token
    }

    #endregion

    #region RevokeTokenAsync Tests

    [Fact]
    public async Task RevokeTokenAsync_CallsRepositoryToRevokeAndSaveChanges()
    {
        // Arrange
        var token = "tokenToRevoke";

        // Act
        await _authService.RevokeTokenAsync(token);

        // Assert
        await _refreshTokenRepository.Received(1).RevokeTokenAsync(token);
        await _refreshTokenRepository.Received(1).SaveChangesAsync();
    }

    #endregion

    #region ChangePasswordAsync Tests

    [Fact]
    public async Task ChangePasswordAsync_WhenNewPasswordsDoNotMatch_ThrowsInvalidOperationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new ChangePasswordRequestDto
        {
            CurrentPassword = "CurrentPassword123!",
            NewPassword = "NewPassword123!",
            ConfirmNewPassword = "DifferentNewPassword123!"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.ChangePasswordAsync(userId, request));
        Assert.Equal("Les nouveaux mots de passe ne correspondent pas.", exception.Message);

        await _userRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>());
    }

    [Fact]
    public async Task ChangePasswordAsync_WhenUserNotFound_ThrowsKeyNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new ChangePasswordRequestDto
        {
            CurrentPassword = "CurrentPassword123!",
            NewPassword = "NewPassword123!",
            ConfirmNewPassword = "NewPassword123!"
        };

        _userRepository.GetByIdAsync(userId).Returns((User?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _authService.ChangePasswordAsync(userId, request));
        Assert.Equal($"Membre {userId} introuvable.", exception.Message);

        await _userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task ChangePasswordAsync_WhenCurrentPasswordIsIncorrect_ThrowsInvalidOperationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new ChangePasswordRequestDto
        {
            CurrentPassword = "WrongCurrentPassword!",
            NewPassword = "NewPassword123!",
            ConfirmNewPassword = "NewPassword123!"
        };

        var user = new User
        {
            Id = userId,
            Pseudo = "UserPseudo",
            Email = "user@test.com",
            FirstName = "First",
            LastName = "Last",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectCurrentPassword!"),
            Birthday = DateTime.UtcNow.AddYears(-20),
            Role = Role.Membre
        };

        _userRepository.GetByIdAsync(userId).Returns(user);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.ChangePasswordAsync(userId, request));
        Assert.Equal("Mot de passe actuel incorrect.", exception.Message);

        await _userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task ChangePasswordAsync_WhenNewPasswordIsSameAsOld_ThrowsInvalidOperationException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new ChangePasswordRequestDto
        {
            CurrentPassword = "CurrentPassword123!",
            NewPassword = "CurrentPassword123!",
            ConfirmNewPassword = "CurrentPassword123!"
        };

        var user = new User
        {
            Id = userId,
            Pseudo = "UserPseudo",
            Email = "user@test.com",
            FirstName = "First",
            LastName = "Last",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CurrentPassword123!"),
            Birthday = DateTime.UtcNow.AddYears(-20),
            Role = Role.Membre
        };

        _userRepository.GetByIdAsync(userId).Returns(user);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.ChangePasswordAsync(userId, request));
        Assert.Equal("Le nouveau mot de passe doit être différent de l'ancien.", exception.Message);

        await _userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task ChangePasswordAsync_WhenRequestIsValid_UpdatesPasswordAndSaves()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var request = new ChangePasswordRequestDto
        {
            CurrentPassword = "CurrentPassword123!",
            NewPassword = "NewPassword123!",
            ConfirmNewPassword = "NewPassword123!"
        };

        var user = new User
        {
            Id = userId,
            Pseudo = "UserPseudo",
            Email = "user@test.com",
            FirstName = "First",
            LastName = "Last",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("CurrentPassword123!"),
            Birthday = DateTime.UtcNow.AddYears(-20),
            Role = Role.Membre
        };

        _userRepository.GetByIdAsync(userId).Returns(user);

        // Act
        await _authService.ChangePasswordAsync(userId, request);

        // Assert
        await _userRepository.Received(1).UpdateAsync(Arg.Is<User>(u =>
            u.Id == userId &&
            BCrypt.Net.BCrypt.Verify(request.NewPassword, u.PasswordHash) &&
            u.UpdatedAt != null
        ));
        await _userRepository.Received(1).SaveChangesAsync();
    }

    #endregion
}
