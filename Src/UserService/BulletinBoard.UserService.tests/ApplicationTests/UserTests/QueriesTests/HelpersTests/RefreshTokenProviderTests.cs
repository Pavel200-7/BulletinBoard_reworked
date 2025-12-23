using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshToken;
using System.Runtime;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.QueriesTests.HelpersTests;

public class RefreshTokenProviderTests
{
    private RefreshTokenProvider _provider;
    private RefreshTokenSettings _settings;

    public RefreshTokenProviderTests()
    {
        _settings = new RefreshTokenSettings() { ExpiresIn = 669600 };
        _provider = new RefreshTokenProvider(_settings);
    }

    [Fact]
    public async Task MustCreateNewRefreshToken()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        // Act
        var result = await _provider.GenerateRefreshToken(userId);

        // Assert
    }
}
