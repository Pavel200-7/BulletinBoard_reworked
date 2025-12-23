using BulletinBoard.UserService.AppServices.Common.Configurations;

namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshToken;

public class RefreshTokenProvider : IRefreshTokenProvider
{
    private RefreshTokenSettings _refreshSettings;

    public RefreshTokenProvider(RefreshTokenSettings refreshSettings)
    {
        _refreshSettings = refreshSettings;
    }

    public async Task<string> GenerateRefreshToken(string userId)
    {
        throw new NotImplementedException();
    }
}
