using BulletinBoard.UserService.AppServices.Common.Configurations;
using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using BulletinBoard.UserService.Domain.Entityes;
using Microsoft.Extensions.Options;

namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshT;

public class RefreshTokenProvider : IRefreshTokenProvider
{
    private readonly IRefreshTokenRepository _tokenRepository;
    private readonly ICommandRepository<RefreshToken> _repository;
    private readonly RefreshTokenSettings _refreshSettings;

    public RefreshTokenProvider(
        IRefreshTokenRepository tokenRepository,
        ICommandRepository<RefreshToken> repository, 
        IOptions<RefreshTokenSettings> refreshSettings)
    {
        _tokenRepository = tokenRepository;
        _repository = repository;
        _refreshSettings = refreshSettings.Value;
    }

    public async Task<string> GenerateTokenAsync(string userId, CancellationToken cancellationToken)
    {
        var oldRefreshTokens = await _tokenRepository.GetRefreshTokensByUserIdAsync(userId, cancellationToken);
        foreach (var oldRefreshToken in oldRefreshTokens)
        {
            await _repository.DeleteAsync(oldRefreshToken.Id, cancellationToken);
        }

        var refreshToken = new RefreshToken(userId,
            DateTime.UtcNow.AddSeconds(_refreshSettings.ExpiresIn));
        await _repository.AddAsync(refreshToken, cancellationToken);

        return refreshToken.Token;
    }
}
