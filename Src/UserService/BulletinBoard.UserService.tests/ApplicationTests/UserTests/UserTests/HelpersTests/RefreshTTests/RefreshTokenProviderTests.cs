using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.AppServices.User.User.Helpers.RefreshT;
using BulletinBoard.UserService.Domain.Entities.RefreshToken;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.UserTests.HelpersTests.RefreshTTests;

public class RefreshTokenProviderTests
{
    private readonly Mock<IQueryRepository<RefreshToken>> _qRepository;
    private Mock<ICommandRepository<RefreshToken>> _cRepository;
    private RefreshTokenSettings _settings;
    private Mock<IOptions<RefreshTokenSettings>> _settingsOptions;
    private RefreshTokenProvider _provider;
    private CancellationToken _cancellationToken;

    public RefreshTokenProviderTests()
    {
        _qRepository = new Mock<IQueryRepository<RefreshToken>>();
        _cRepository = new Mock<ICommandRepository<RefreshToken>>();

        _settings = new RefreshTokenSettings() { ExpiresIn = 669600 };
        _settingsOptions = new Mock<IOptions<RefreshTokenSettings>>();
        _settingsOptions.Setup(x => x.Value).Returns(_settings);

        _provider = new RefreshTokenProvider(
            _qRepository.Object, 
            _cRepository.Object, _settingsOptions.Object);

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
        _qRepository.Verify(tr => tr.GetWithSpecificationAsync(
            It.IsAny<Expression<Func<RefreshToken, bool>>>(), 
            _cancellationToken));
        _cRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), _cancellationToken));
    }

    [Fact]
    public async Task MustCreateNewRefreshTokenAndWriteIt()
    {
        // Arrange
        string userId = CreateId();

        // Act
        var result = await _provider.GenerateTokenAsync(userId, _cancellationToken);

        // Assert
        _cRepository.Verify(r => r.AddAsync(It.IsAny<RefreshToken>(), _cancellationToken));
    }

    private void SetupMock()
    {
        var oldRefreshTokens = CreateRefreshtoken();
        _qRepository.Setup(tr => tr.GetWithSpecificationAsync(
            It.IsAny<Expression<Func<RefreshToken, bool>>>(),
            _cancellationToken))
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
