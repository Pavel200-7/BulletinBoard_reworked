using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshT;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using BulletinBoard.UserService.Domain.Entityes;
using MediatR;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Queries.Refresh;

public class RefreshQueryHandler : IRequestHandler<RefreshQuery, RefreshQResponse>
{
    private readonly ILogger<RefreshQueryHandler> _logger;
    private readonly IRefreshTokenRepository _repository;
    private readonly IJWTProvider _jWTProvider;
    private readonly IRefreshTokenProvider _refreshTProvider;

    public RefreshQueryHandler(
        ILogger<RefreshQueryHandler> logger, 
        IRefreshTokenRepository repository,
        IJWTProvider jWTProvider,
        IRefreshTokenProvider refreshTProvider)
    {
        _logger = logger;
        _repository = repository;
        _jWTProvider = jWTProvider;
        _refreshTProvider = refreshTProvider;
    }

    public async Task<RefreshQResponse> Handle(RefreshQuery request, CancellationToken cancellationToken)
    {
        var refreshTokenData = await _repository.GetRefreshTokensByTokenStringAsync(request.RefreshToken, cancellationToken);
        if (refreshTokenData is null)
        {
            throw new NotFoundException("Refresh токен с такой строкой не найден");
        }

        var tokenData = await _jWTProvider.GenerateTokenAsync(refreshTokenData.UserId, cancellationToken);
        var refreshToken = await _refreshTProvider.GenerateTokenAsync(refreshTokenData.UserId, cancellationToken);

        return new RefreshQResponse()
        {
            TokenType = tokenData.TokenType,
            AccessToken = tokenData.AccessToken,
            ExpiresIn = tokenData.ExpiresIn,
            RefreshToken = refreshToken,
        };
    }
}
