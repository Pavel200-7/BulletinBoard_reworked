using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.AppServices.User.User.Helpers.JWT;
using BulletinBoard.UserService.AppServices.User.User.Helpers.RefreshT;
using BulletinBoard.UserService.Domain.Entities.RefreshToken;
using BulletinBoard.UserService.Domain.Entities.RefreshToken.Helpers.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.User.Queries.Refresh;

public class RefreshQueryHandler : IRequestHandler<RefreshQuery, RefreshQResponse>
{
    private readonly ILogger<RefreshQueryHandler> _logger;
    private readonly IQueryRepository<RefreshToken> _repository;
    private readonly IJWTProvider _jWTProvider;
    private readonly IRefreshTokenProvider _refreshTProvider;

    public RefreshQueryHandler(
        ILogger<RefreshQueryHandler> logger,
        IQueryRepository<RefreshToken> repository,
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
        var filter = new TokenStringSpecification<RefreshToken>(request.RefreshToken);
        RefreshToken refreshTokenData = await _repository
            .GetFirstWithSpecificationAsync(filter.ToExpression(), cancellationToken)
            .ThrowNotFoundIfNull("Refresh токен с такой строкой не найден");

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
