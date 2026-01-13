using BulletinBoard.UserService.AppServices.Common.Behaviors.Transaction;
using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.Domain.Entities.RefreshToken;
using BulletinBoard.UserService.Domain.Entities.RefreshToken.Helpers.Specifications;
using Microsoft.Extensions.Options;


namespace BulletinBoard.UserService.AppServices.User.User.Helpers.RefreshT;

[Transaction]
public class RefreshTokenProvider : IRefreshTokenProvider
{
    private readonly IQueryRepository<RefreshToken> _qRepository;
    private readonly ICommandRepository<RefreshToken> _cRepository;
    private readonly RefreshTokenSettings _refreshSettings;

    public RefreshTokenProvider(
        IQueryRepository<RefreshToken> qRepository,
        ICommandRepository<RefreshToken> cRepository, 
        IOptions<RefreshTokenSettings> refreshSettings)
    {
        _qRepository = qRepository;
        _cRepository = cRepository;
        _refreshSettings = refreshSettings.Value;
    }

    public async Task<string> GenerateTokenAsync(string userId, CancellationToken cancellationToken)
    {
        var filter = new UserIdSpecification<RefreshToken>(userId);
        var oldRefreshTokens = await _qRepository.GetWithSpecificationAsync(filter.ToExpression(), cancellationToken);
        foreach (var oldRefreshToken in oldRefreshTokens)
        {
            await _cRepository.DeleteAsync(oldRefreshToken.Id, cancellationToken);
        }

        var refreshToken = new RefreshToken(userId,
            DateTime.UtcNow.AddSeconds(_refreshSettings.ExpiresIn));
        await _cRepository.AddAsync(refreshToken, cancellationToken);

        return refreshToken.Token;
    }
}
