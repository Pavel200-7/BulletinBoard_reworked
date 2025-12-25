using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshT;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn;
using BulletinBoard.UserService.AppServices.User.Queries.Refresh;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using BulletinBoard.UserService.Domain.Entityes;
using Microsoft.Extensions.Logging;
using Moq;
using System.Runtime;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.QueriesTests.RefreshQueryTests;

public class RefreshQueryHandlerTests
{
    private Mock<ILogger<RefreshQueryHandler>> _logger;
    private Mock<IRefreshTokenRepository> _repository;
    private Mock<IJWTProvider> _jwtProvider;
    private Mock<IRefreshTokenProvider> _refreshTProvider;
    private RefreshQueryHandler _handler;
    private CancellationToken _cancellationToken;

    public RefreshQueryHandlerTests()
    {
        _logger = new Mock<ILogger<RefreshQueryHandler>>();
        _repository = new Mock<IRefreshTokenRepository>();
        _jwtProvider = new Mock<IJWTProvider>();
        _refreshTProvider = new Mock<IRefreshTokenProvider>();
        _handler = new RefreshQueryHandler(_logger.Object, _repository.Object, _jwtProvider.Object, _refreshTProvider.Object);
        _cancellationToken = CancellationToken.None;

        SetUpMock();
    }

    [Fact]
    public async Task MustSearchForTokenData()
    {
        // Arrange 
        var query = CreateQuery();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        // Assert
        _repository.Verify(r => r.GetRefreshTokensByTokenStringAsync(query.RefreshToken, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task MustThrowWhenTokenNotFound()
    {
        // Arrange 
        var query = CreateQuery();
        _repository.Setup(r => r.GetRefreshTokensByTokenStringAsync(query.RefreshToken, _cancellationToken))
           .ReturnsAsync((RefreshToken)null!);

        // Act
        var act =  async() => await _handler.Handle(query, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
    }

    [Fact]
    public async Task MustCreateNewJWT()
    {
        // Arrange
        var query = CreateQuery();

        //Act
        var result = await _handler.Handle(query, _cancellationToken);

        //Assert
        _jwtProvider.Verify(jwtP => jwtP.GenerateTokenAsync(It.IsAny<string>(), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task MustCreateNewRefreshToken()
    {
        // Arrange
        var query = CreateQuery();

        //Act
        var result = await _handler.Handle(query, _cancellationToken);

        //Assert
        _refreshTProvider.Verify(jwtP => jwtP.GenerateTokenAsync(It.IsAny<string>(), _cancellationToken), Times.Once);
    }

    private void SetUpMock()
    {
        var refreshToken = CreateRefreshToken();
        _repository.Setup(r => r.GetRefreshTokensByTokenStringAsync(refreshToken, _cancellationToken))
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
