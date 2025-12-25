using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshT;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using BulletinBoard.UserService.Domain.Entityes;
using Microsoft.Extensions.Options;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.QueriesTests.HelpersTests;

public class RefreshTokenProviderTests
{
    private readonly Mock<IRefreshTokenRepository> _tokenRepository;
    private Mock<ICommandRepository<RefreshToken>> _repository;
    private RefreshTokenSettings _settings;
    private Mock<IOptions<RefreshTokenSettings>> _settingsOptions;
    private RefreshTokenProvider _provider;
    private CancellationToken _cancellationToken;

    public RefreshTokenProviderTests()
    {
        _tokenRepository = new Mock<IRefreshTokenRepository>();
        _repository = new Mock<ICommandRepository<RefreshToken>>();

        _settings = new RefreshTokenSettings() { ExpiresIn = 669600 };
        _settingsOptions = new Mock<IOptions<RefreshTokenSettings>>();
        _settingsOptions.Setup(x => x.Value).Returns(_settings);

        _provider = new RefreshTokenProvider(_tokenRepository.Object, _repository.Object, _settingsOptions.Object);

        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task MustDeleteOldRefreshTokenAndWriteIt()
    {
        // Arrange
        string userId = CreateId();

        // Act
        var result = await _provider.GenerateTokenAsync(userId, _cancellationToken);

        // Assert
        _tokenRepository.Verify(tr => tr.GetRefreshTokensByUserIdAsync(userId, _cancellationToken));
        _repository.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), _cancellationToken));
    }

    [Fact]
    public async Task MustCreateNewRefreshTokenAndWriteIt()
    {
        // Arrange
        string userId = CreateId();

        // Act
        var result = await _provider.GenerateTokenAsync(userId, _cancellationToken);

        // Assert
        _repository.Verify(r => r.AddAsync(It.IsAny<RefreshToken>(), _cancellationToken));
    }

    private void SetupMock()
    {
        var oldRefreshTokens = CreateRefreshtoken();
        _tokenRepository.Setup(t => t.GetRefreshTokensByUserIdAsync(It.IsAny<string>(), _cancellationToken))
            .ReturnsAsync(oldRefreshTokens);

    }

    public List<RefreshToken> CreateRefreshtoken()
    {
        var userId = CreateId();
        var refreshTokens = new List<RefreshToken>()
        {
            new RefreshToken(CreateId(), 
                DateTime.UtcNow.AddSeconds(_settings.ExpiresIn))
        };

        return refreshTokens;
    }

    private string CreateId()
    {
        return "esduaavhsfbaSJsvflaLZBJD,fdgjhkDLBJcd345zkdj1dfajbuger43yi";
    }
}
