using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshT;
using BulletinBoard.UserService.Domain.Entityes;
using Microsoft.Extensions.Options;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.QueriesTests.HelpersTests;

public class RefreshTokenProviderTests
{
    private RefreshTokenProvider _provider;
    private Mock<ICommandRepository<RefreshToken>> _repository;
    private Mock<IOptions<RefreshTokenSettings>> _settings;
    private CancellationToken _cancellationToken;

    public RefreshTokenProviderTests()
    {
        var settings = new RefreshTokenSettings() { ExpiresIn = 669600 };
        _settings = new Mock<IOptions<RefreshTokenSettings>>();
        _settings.Setup(x => x.Value).Returns(settings);

        _repository = new Mock<ICommandRepository<RefreshToken>>();
        _provider = new RefreshTokenProvider(_repository.Object, _settings.Object);

        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task MustCreateNewRefreshTokenAndWriteIt()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        // Act
        var result = await _provider.GenerateRefreshTokenAsync(userId, _cancellationToken);

        // Assert
        _repository.Verify(r => r.AddAsync(It.IsAny<RefreshToken>(), _cancellationToken));
    }
}
