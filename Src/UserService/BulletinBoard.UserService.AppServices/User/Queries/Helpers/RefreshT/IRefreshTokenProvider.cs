namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshT;

public interface IRefreshTokenProvider
{
    public Task<string> GenerateTokenAsync(string userId, CancellationToken cancellationToken);
}
