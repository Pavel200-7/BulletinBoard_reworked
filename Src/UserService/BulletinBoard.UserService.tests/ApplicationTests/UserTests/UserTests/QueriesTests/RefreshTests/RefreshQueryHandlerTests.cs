using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.AppServices.User.User.Helpers.JWT;
using BulletinBoard.UserService.AppServices.User.User.Helpers.RefreshT;
using BulletinBoard.UserService.AppServices.User.User.Queries.Refresh;
using BulletinBoard.UserService.Domain.Entities.RefreshToken;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.User.QueriesTests.RefreshTests;

public class RefreshQueryHandlerTests
{
    private Mock<ILogger<RefreshQueryHandler>> _logger;
    private Mock<IQueryRepository<RefreshToken>> _repository;
    private Mock<IJWTProvider> _jwtProvider;
    private Mock<IRefreshTokenProvider> _refreshTProvider;
    private RefreshQueryHandler _handler;
    private CancellationToken _cancellationToken;

    public RefreshQueryHandlerTests()
    {
        _logger = new Mock<ILogger<RefreshQueryHandler>>();
        _repository = new Mock<IQueryRepository<RefreshToken>>();
        _jwtProvider = new Mock<IJWTProvider>();
        _refreshTProvider = new Mock<IRefreshTokenProvider>();
        _handler = new RefreshQueryHandler(_logger.Object,
            _repository.Object, 
            _jwtProvider.Object, _refreshTProvider.Object);
        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task SearchForTokenData()
    {
        // Arrange 
        var query = CreateQuery();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        // Assert
        _repository.Verify(r => r.GetFirstWithSpecificationAsync(
            It.IsAny<Expression<Func<RefreshToken, bool>>>(),
            _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task ThrowWhenTokenNotFound()
    {
        // Arrange 
        var query = CreateQuery();
        _repository.Setup(r => r.GetFirstWithSpecificationAsync(
            It.IsAny<Expression<Func<RefreshToken, bool>>>(),
            _cancellationToken))
           .ReturnsAsync((RefreshToken)null!);

        // Act
        var act =  async() => await _handler.Handle(query, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
    }

    [Fact]
    public async Task CreateNewJWT()
    {
        // Arrange
        var query = CreateQuery();

        //Act
        var result = await _handler.Handle(query, _cancellationToken);

        //Assert
        _jwtProvider.Verify(jwtP => jwtP.GenerateTokenAsync(It.IsAny<string>(), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CreateNewRefreshToken()
    {
        // Arrange
        var query = CreateQuery();

        //Act
        var result = await _handler.Handle(query, _cancellationToken);

        //Assert
        _refreshTProvider.Verify(jwtP => jwtP.GenerateTokenAsync(It.IsAny<string>(), _cancellationToken), Times.Once);
    }

    private void SetupMock()
    {
        var refreshToken = CreateRefreshToken();
        _repository.Setup(r => r.GetFirstWithSpecificationAsync(
            It.IsAny<Expression<Func<RefreshToken, bool>>>(),
            _cancellationToken))
            .ReturnsAsync(CreateRefreshTokenData());

        _jwtProvider.Setup(g => g.GenerateTokenAsync(CreateUserId(), _cancellationToken))
            .ReturnsAsync(CreateAccessTokenData());

        _refreshTProvider.Setup(rp => rp.GenerateTokenAsync(CreateUserId(), _cancellationToken))
            .ReturnsAsync(CreateRefreshToken());
    }

    public RefreshQuery CreateQuery()
    {
        return new RefreshQuery()
        {
            RefreshToken = CreateRefreshToken()
        };
    }

    public string CreateRefreshToken()
    {
        return "MyRefreshToken";
    }

    public RefreshToken CreateRefreshTokenData()
    {
        var userId = CreateUserId();     
        var exprisesIn = 669600;
        var refreshToken = new RefreshToken(userId,
            DateTime.UtcNow.AddSeconds(exprisesIn));

        refreshToken.Token = CreateRefreshToken();

        return refreshToken;
    }

    public string CreateUserId()
    {
        return "SomeUserId";
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
}
