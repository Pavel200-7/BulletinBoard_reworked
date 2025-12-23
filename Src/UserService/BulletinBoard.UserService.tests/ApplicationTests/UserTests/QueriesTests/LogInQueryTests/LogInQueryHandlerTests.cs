using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn;
using BulletinBoard.UserService.tests.ApplicationTests.UserTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.QueriesTests.LogInQueryTests;

public class LogInQueryHandlerTests
{
    private Mock<ILogger<LogInQueryHandler>> MockLogger;
    private Mock<IMapper> MockMapper;
    private Mock<UserManager<IdentityUser>> MockUserManager;
    private Mock<SignInManager<IdentityUser>> MockSignInManager;
    private Mock<IJWTProvider> MockJWTGenerator;
    private LogInQueryHandler handler;
    private CancellationToken cancellationToken;

    public LogInQueryHandlerTests()
    {
        MockLogger = new Mock<ILogger<LogInQueryHandler>>();
        MockMapper = new Mock<IMapper>();
        MockJWTGenerator = new Mock<IJWTProvider>();

        var userManagerInitializer = new IdentityMockInitializer();
        MockUserManager = userManagerInitializer.GetMockUserManager<IdentityUser>();
        MockSignInManager = userManagerInitializer.GetMockSignInManager(MockUserManager);

        handler = new LogInQueryHandler(
            MockLogger.Object, 
            MockMapper.Object, 
            MockUserManager!.Object, 
            MockSignInManager!.Object, 
            MockJWTGenerator!.Object);

        cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task MustChechEmail()
    {
        // Arrange
        var query = CreateQuery();
        MockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        // Act
        var act = async() => await handler.Handle(query, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
        MockUserManager.Verify(um => um.FindByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task MustChechPassword()
    {
        // Arrange
        var query = CreateQuery();
        MockSignInManager.Setup(sim => sim.PasswordSignInAsync(
            It.IsAny<IdentityUser>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()
            ))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var act = async () => await handler.Handle(query, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
        MockSignInManager.Verify(sim => sim.PasswordSignInAsync(
            It.IsAny<IdentityUser>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()
            ), Times.Once);
    }

    [Fact]
    public async Task MustMakeToken()
    {
        // Arrange
        var query = CreateQuery();
        var user = CreateUser();
        var userRoles = new List<string> { "User", "Admin" };

        MockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(userRoles);

        MockUserManager.Setup(um => um.GenerateUserTokenAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
            .ReturnsAsync("mocked-refresh-token-12345");

        var expectedTokenData = new TokenData
        {
            TokenType = "Bearer",
            AccessToken = "mocked-access-token-jwt-string",
            ExpiresIn = 3600
        };

        MockJWTGenerator.Setup(g => g.GenerateToken(It.IsAny<ClaimsData>()))
            .Returns(expectedTokenData);

        // Act
        var result = await handler.Handle(query, cancellationToken);

        // Assert
        MockJWTGenerator.Verify(
            g => g.GenerateToken(It.Is<ClaimsData>(cd =>
                cd.UserId == user.Id &&
                cd.Email == user.Email)),
            Times.Once);

        MockUserManager.Verify(
            um => um.GenerateUserTokenAsync(
                It.Is<IdentityUser>(u => u.Id == user.Id),
                "RefreshTokenProvider",
                "Refresh"),
            Times.Once);

        Assert.NotNull(result);
        Assert.Equal(expectedTokenData.TokenType, result.TokenType);
        Assert.Equal(expectedTokenData.AccessToken, result.AccessToken);
        Assert.Equal(expectedTokenData.ExpiresIn, result.ExpiresIn);
        Assert.Equal("mocked-refresh-token-12345", result.RefreshToken);
    }


    private void SetupMock()
    {
        var user = CreateUser();
        MockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        MockSignInManager.Setup(sim => sim.PasswordSignInAsync(
            It.IsAny<IdentityUser>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()
            ))
            .ReturnsAsync(SignInResult.Success);
    }

    private LogInQuery CreateQuery()
    {
        return new LogInQuery()
        {
            Email = "Email@email.com",
            Password = "Password123"
        };
    }

    private IdentityUser CreateUser()
    {
        return new IdentityUser()
        {
            Id = "esduaavhsfbaSJsvflaLZBJD,fdgjhkDLBJcd345zkdj1dfajbuger43yi",
            UserName = "User12432",
            Email = "email@email.com",
            PhoneNumber = "+7 (978) 123-45-67",
        };
    }
}
