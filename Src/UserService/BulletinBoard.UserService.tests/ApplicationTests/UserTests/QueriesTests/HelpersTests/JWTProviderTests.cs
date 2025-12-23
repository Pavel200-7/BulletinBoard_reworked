using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;
using Microsoft.Extensions.Options;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.QueriesTests.HelpersTests;

public class JWTProviderTests
{
    private Mock<IOptions<JwtSettings>> MockJwtSettings;
    private JWTProvider JWTProvider;

    public JWTProviderTests()
    {
        MockJwtSettings = new Mock<IOptions<JwtSettings>>();

        var jwtSettings = new JwtSettings
        {
            Key = "TestKey____________________________________________________",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            ExpiresIn = 3600
        };

        MockJwtSettings.Setup(x => x.Value).Returns(jwtSettings);
        JWTProvider = new JWTProvider(MockJwtSettings.Object);
    }

    [Fact]
    public void MustCreateObjectWithAuthData()
    {
        // Arrange
        var claimsData = new ClaimsData()
        {
            UserId = Guid.NewGuid().ToString(),
            Email = "Email@email.com",
            Roles = new List<string> { "User" }
        };
        var expectedExpiresIn = 3600;

        // Act
        var result = JWTProvider.GenerateToken(claimsData);

        // Assert
        Assert.Equal("Bearer", result.TokenType);
        Assert.Equal(expectedExpiresIn, result.ExpiresIn);
        Assert.NotNull(result.AccessToken);
        Assert.NotEmpty(result.AccessToken);
    }
}
