using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshT;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn;
using BulletinBoard.UserService.tests.ApplicationTests.UserTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.QueriesTests.LogInQueryTests;

public class LogInQueryHandlerTests
{
    private Mock<ILogger<LogInQueryHandler>> _logger;
    private Mock<IMapper> _mapper;
    private Mock<UserManager<IdentityUser>> _userManager;
    private Mock<SignInManager<IdentityUser>> _signInManager;
    private Mock<IJWTProvider> _JWTProvider;
    private Mock<IRefreshTokenProvider> _refreshTProvider;

    private LogInQueryHandler handler;
    private CancellationToken _cancellationToken;

    public LogInQueryHandlerTests()
    {
        _logger = new Mock<ILogger<LogInQueryHandler>>();
        _mapper = new Mock<IMapper>();
        _JWTProvider = new Mock<IJWTProvider>();
        _refreshTProvider = new Mock<IRefreshTokenProvider>();   

        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();
        _signInManager = userManagerInitializer.GetMockSignInManager(_userManager);

        handler = new LogInQueryHandler(
            _logger.Object, 
            _mapper.Object, 
            _userManager!.Object, 
            _signInManager!.Object, 
            _JWTProvider!.Object,
            _refreshTProvider!.Object);

        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task MustChechEmail()
    {
        // Arrange
        var query = CreateQuery();
        _userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        // Act
        var act = async() => await handler.Handle(query, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
        _userManager.Verify(um => um.FindByEmailAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task MustChechPassword()
    {
        // Arrange
        var query = CreateQuery();
        _signInManager.Setup(sim => sim.PasswordSignInAsync(
            It.IsAny<IdentityUser>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()
            ))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var act = async () => await handler.Handle(query, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
        _signInManager.Verify(sim => sim.PasswordSignInAsync(
            It.IsAny<IdentityUser>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()
            ), Times.Once);
    }

    [Fact]
    public async Task MustMakeJWTToken()
    {
        // Arrange
        var query = CreateQuery();
        var user = CreateUser();
        var expectedTokenData = CreateAccessTokenData();

        // Act
        var result = await handler.Handle(query, _cancellationToken);

        // Assert
        _JWTProvider.Verify(g => g.GenerateToken(user.Id, _cancellationToken), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(expectedTokenData.TokenType, result.TokenType);
        Assert.Equal(expectedTokenData.AccessToken, result.AccessToken);
        Assert.Equal(expectedTokenData.ExpiresIn, result.ExpiresIn);
    }

    [Fact]
    public async Task MustRefreshToken()
    {
        // Arrange
        var query = CreateQuery();
        var user = CreateUser();
        var expectedRefreshToken = CreateRefreshToken();

        // Act
        var result = await handler.Handle(query, _cancellationToken);

        // Assert
        _refreshTProvider.Verify(rp => rp.GenerateRefreshTokenAsync(user.Id, _cancellationToken), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(expectedRefreshToken, result.RefreshToken);
    }


    private void SetupMock()
    {
        var user = CreateUser();
        _userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _signInManager.Setup(sim => sim.PasswordSignInAsync(
            It.IsAny<IdentityUser>(),
            It.IsAny<string>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()
            ))
            .ReturnsAsync(SignInResult.Success);

        var userRoles = new List<string> { "User", "Admin" };
        _userManager.Setup(um => um.GetRolesAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(userRoles);

        _JWTProvider.Setup(g => g.GenerateToken(user.Id, _cancellationToken))
            .ReturnsAsync(CreateAccessTokenData());

        _refreshTProvider.Setup(rp => rp.GenerateRefreshTokenAsync(user.Id, _cancellationToken))
            .ReturnsAsync(CreateRefreshToken());
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

    private TokenData CreateAccessTokenData()
    {
        return new TokenData()
        {
            TokenType = "Bearer",
            AccessToken = "mocked-access-token-jwt-string",
            ExpiresIn = 3600
        };
    }

    private string CreateRefreshToken()
    {
        return "mocked-refresh-token-12345";
    }
}
