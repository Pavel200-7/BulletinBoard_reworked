using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn.Helpers.JWTGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.QueriesTests.LogInQueryTests;

public class JWTGeneratorTests
{
    private Mock<IOptions<JwtSettings>> MockJwtSettings;
    private JWTGenerator jWTGenerator;

    public JWTGeneratorTests()
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
        jWTGenerator = new JWTGenerator(MockJwtSettings.Object);
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
        var result = jWTGenerator.GenerateToken(claimsData);

        // Assert
        Assert.Equal("Bearer", result.TokenType);
        Assert.Equal(expectedExpiresIn, result.ExpiresIn);
        Assert.NotNull(result.AccessToken);
        Assert.NotEmpty(result.AccessToken);
    }
}
