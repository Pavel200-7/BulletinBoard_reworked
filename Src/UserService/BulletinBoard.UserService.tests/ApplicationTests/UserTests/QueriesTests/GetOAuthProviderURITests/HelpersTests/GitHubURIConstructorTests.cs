using BulletinBoard.NotificationService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;


namespace BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.QueriesTests.GetOAuthProviderURITests.HelpersTests;

public class GitHubURIConstructorTests
{
    private readonly Mock<ILogger<GitHubURIConstructor>> _logger;
    private readonly Mock<IOptions<GitHubOAuthSettings>> _options;
    private readonly GitHubURIConstructor _constructor;

    public GitHubURIConstructorTests()
    {
        _logger = new Mock<ILogger<GitHubURIConstructor>>();
        _options = new Mock<IOptions<GitHubOAuthSettings>>();
        var settings = new GitHubOAuthSettings
        {
            ClientId = "Iv1.abc123def456ghi78",
            ClientSecret = "secret123",
            RedirectURI = "http://localhost:8090/api/v1/oauth/login_callback/github",
        };
        _options.Setup(o => o.Value).Returns(settings);  
        _constructor = new GitHubURIConstructor(_logger.Object, _options.Object);

        SetupMock();
    }

    [Fact]
    public void CreateURI_WithValidData_ReturnsCorrectGitHubUrl()
    {
        // Arrange
        var uriData = new URIData { State = "test_state_123" };
        var expectedUrl = "https://github.com/login/oauth/authorize?" +
            "client_id=Iv1.abc123def456ghi78&" +
            "redirect_uri=http%3A%2F%2Flocalhost%3A8090%2Fapi%2Fv1%2Foauth%2Flogin_callback%2Fgithub&" +
            "state=test_state_123&" +
            "allow_signup=false";

        // Act
        var result = _constructor.CreateURI(uriData);

        // Assert
        Assert.Equal(expectedUrl, result);
    }

    private void SetupMock()
    {  
    }
}
