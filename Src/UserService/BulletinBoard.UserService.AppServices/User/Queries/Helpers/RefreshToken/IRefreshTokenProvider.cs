namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshToken;

public interface IRefreshTokenProvider
{
    public Task<string> GenerateRefreshToken(string userId);
}
