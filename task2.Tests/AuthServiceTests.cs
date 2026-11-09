using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using task2.Domain.Entities;
using task2.Dtos.Auth;
using task2.Exceptions;
using task2.Services.Implementations;
using Xunit;

namespace task2.Tests;

public class AuthServiceTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        // Налаштовуємо Mock для UserManager (він потребує IUserStore у конструкторі)
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        // Налаштовуємо Mock для IConfiguration (потрібен для генерації JWT-токена)
        _configMock = new Mock<IConfiguration>();
        _configMock.Setup(c => c["Jwt:Key"]).Returns("super_secret_key_that_is_long_enough_32_chars");
        _configMock.Setup(c => c["Jwt:Issuer"]).Returns("test_issuer");
        _configMock.Setup(c => c["Jwt:Audience"]).Returns("test_audience");
        _configMock.Setup(c => c["Jwt:ExpiresMinutes"]).Returns("60");

        _authService = new AuthService(_userManagerMock.Object, _configMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_EmailAlreadyExists_ThrowsBadRequestException()
    {
        // Arrange
        var dto = new RegisterDto { Email = "test@example.com", UserName = "testuser", Password = "password123" };

        // Імітуємо, що користувач з таким email вже існує в базі
        _userManagerMock.Setup(m => m.FindByEmailAsync(dto.Email))
            .ReturnsAsync(new User());

        // Act & Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _authService.RegisterAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_InvalidCredentials_ThrowsUnauthorizedException()
    {
        // Arrange
        var dto = new LoginDto { UserNameOrEmail = "wrong@example.com", Password = "wrongpassword" };

        // Імітуємо, що користувача не знайдено ні за логіном, ні за email
        _userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((User)null!);
        _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedException>(() => _authService.LoginAsync(dto));
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsAuthResponse()
    {
        // Arrange
        var dto = new LoginDto { UserNameOrEmail = "valid@example.com", Password = "password123" };
        var user = new User { Id = Guid.NewGuid(), UserName = "validuser", Email = "valid@example.com" };

        // Імітуємо успішний пошук та правильний пароль
        _userManagerMock.Setup(m => m.FindByEmailAsync(dto.UserNameOrEmail)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, dto.Password)).ReturnsAsync(true);

        // Act
        var result = await _authService.LoginAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Token); // Перевіряємо, що JWT-токен згенеровано
        Assert.Equal(user.Id.ToString(), result.UserId);
    }
}