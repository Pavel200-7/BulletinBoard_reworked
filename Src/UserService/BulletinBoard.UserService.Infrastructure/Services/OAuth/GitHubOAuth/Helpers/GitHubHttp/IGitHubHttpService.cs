namespace BulletinBoard.NotificationService.Infrastructure.Services.OAuth.GitHubOAuth.Helpers.GitHubHttp;

public interface IGitHubHttpService
{
    public Task<string> GetAccessToken(string code, CancellationToken cancellationToken);
    public Task<string> GetUserEmail(string accessToken, CancellationToken cancellationToken);
}
