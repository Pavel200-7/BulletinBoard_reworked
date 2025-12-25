namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshT;

public interface IRefreshTokenProvider
{
    public Task<string> GenerateRefreshTokenAsync(string userId, CancellationToken cancellationToken);
}
