using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;
using BulletinBoard.UserService.tests.ApplicationTests.UserTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.QueriesTests.HelpersTests;

public class JWTProviderTests
{
    private Mock<UserManager<IdentityUser>> _userManager;
    private JwtSettings _jwtSettings;
    private Mock<IOptions<JwtSettings>> _JwtSettingsOptions;
    private JWTProvider _provider;
    private CancellationToken _cancellationToken;

    public JWTProviderTests()
    {
        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();

        _JwtSettingsOptions = new Mock<IOptions<JwtSettings>>();
        _jwtSettings = new JwtSettings
        {
            Key = "TestKey____________________________________________________",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiresIn = 3600
        };
        _JwtSettingsOptions.Setup(x => x.Value).Returns(_jwtSettings);

        _provider = new JWTProvider(_userManager.Object, _JwtSettingsOptions.Object);

        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public void MustGetUserData()
    {
        // Arrange
        var id = CreateId();

        // Act
        var result = _provider.GenerateToken(id, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.FindByIdAsync(id), Times.Once());
        _userManager.Verify(um => um.GetRolesAsync(It.IsAny<IdentityUser>()), Times.Once());
    }

    [Fact]
    public async Task MustCreateObjectWithAuthData()
    {
        // Arrange
        var id = CreateId();
        var expectedExpiresIn = _jwtSettings.ExpiresIn;

        // Act
        var result = await _provider.GenerateToken(id, _cancellationToken);

        // Assert
        Assert.Equal("Bearer", result.TokenType);
        Assert.Equal(expectedExpiresIn, result.ExpiresIn);
        Assert.NotNull(result.AccessToken);
        Assert.NotEmpty(result.AccessToken);
    }

    [Fact]
    public async Task MustThrowWhenUserNotFound()
    {
        // Arrange
        var id = CreateId();
        var expectedExpiresIn = _jwtSettings.ExpiresIn;
        _userManager.Setup(um => um.FindByIdAsync(id))
            .ReturnsAsync((IdentityUser)null!);

        // Act
        var act = async() => await _provider.GenerateToken(id, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
    }

    private void SetupMock()
    {
        var user = CreateUser();
        _userManager.Setup(um => um.FindByIdAsync(user.Id))
            .ReturnsAsync(user);

        var userRoles = new List<string>() { "User" };
        _userManager.Setup(um => um.GetRolesAsync(user))
            .ReturnsAsync(userRoles);
    }

    private IdentityUser CreateUser()
    {
        return new IdentityUser()
        {
            Id = CreateId(),
            UserName = "User12432",
            Email = "email@email.com",
            PhoneNumber = "+7 (978) 123-45-67",
        };
    }

    private string CreateId()
    {
        return "esduaavhsfbaSJsvflaLZBJD,fdgjhkDLBJcd345zkdj1dfajbuger43yi";
    }
}
