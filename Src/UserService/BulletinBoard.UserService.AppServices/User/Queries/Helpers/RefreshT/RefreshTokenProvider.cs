using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.Domain.Entityes;
using Microsoft.Extensions.Options;

namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshT;

public class RefreshTokenProvider : IRefreshTokenProvider
{
    private readonly ICommandRepository<RefreshToken> _repository;
    private readonly RefreshTokenSettings _refreshSettings;

    public RefreshTokenProvider(ICommandRepository<RefreshToken> repository, IOptions<RefreshTokenSettings> refreshSettings)
    {
        _repository = repository;
        _refreshSettings = refreshSettings.Value;
    }

    public async Task<string> GenerateRefreshTokenAsync(string userId, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken(userId, 
            DateTime.UtcNow.AddSeconds(_refreshSettings.ExpiresIn));
        await _repository.AddAsync(refreshToken, cancellationToken);
        return refreshToken.Token;
    }
}
