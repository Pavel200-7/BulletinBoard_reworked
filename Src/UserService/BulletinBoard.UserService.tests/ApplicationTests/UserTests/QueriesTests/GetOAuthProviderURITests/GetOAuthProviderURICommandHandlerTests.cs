using BulletinBoard.UserService.AppServices.User.Enum;
using BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI;
using BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor;
using BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor.Factories;
using Microsoft.Extensions.Logging;
using Moq;


namespace BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.QueriesTests.GetOAuthProviderURITests;

public class GetOAuthProviderURICommandHandlerTests
{
    private readonly Mock<ILogger<GetOAuthProviderURIQueryHandler>> _logger;
    private readonly Mock<IURIConstructorFactory> _uriConstructorFactory;
    private readonly Mock<IURIConstructor> _uriConstructor;
    private readonly GetOAuthProviderURIQueryHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public GetOAuthProviderURICommandHandlerTests()
    {
        _logger = new Mock<ILogger<GetOAuthProviderURIQueryHandler>>();
        _uriConstructorFactory = new Mock<IURIConstructorFactory>();
        _uriConstructor = new Mock<IURIConstructor>();
        _handler = new GetOAuthProviderURIQueryHandler(_logger.Object, _uriConstructorFactory.Object);
        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task CreateGithubOAuthURI()
    {
        // Arrange
        var command = CreateCommand();
        var expectedURL = $"SomeUrl";
        _uriConstructor.Setup(uc => uc.CreateURI(It.IsAny<URIData>()))
            .Returns(expectedURL);
        _uriConstructorFactory.Setup(ucf => ucf.CreateConstructor(OAuthProviders.GitHub))
            .Returns(_uriConstructor.Object);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.Equal(expectedURL, result.ProviderURI);
    }

    private void SetupMock()
    {
    }

    private GetOAuthProviderURIQuery CreateCommand(string provider = OAuthProviders.GitHub)
    {
        return new GetOAuthProviderURIQuery(provider, "SomeState");
    }
}
