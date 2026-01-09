using BulletinBoard.NotificationService.Infrastructure.Services.OAuth.GitHubOAuth.Helpers.GitHubHttp;
using BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister.Helpers.OAuth;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.Infrastructure.Services.OAuth.GitHubOAuth;

public class GitHubOAuthService : IOAuthService
{
    private readonly ILogger<GitHubOAuthService> _logger;
    private readonly IGitHubHttpService _gitHubHttpService;

    public GitHubOAuthService(
        ILogger<GitHubOAuthService> logger,
        IGitHubHttpService gitHubHttpService)
    {
        _logger = logger;
        _gitHubHttpService = gitHubHttpService;
    }

    public async Task<UserRegistrationData> GetRegistrationDataFromProviderAsync(string code, CancellationToken cancellationToken)
    {
        string accessToken = await _gitHubHttpService.GetAccessToken(code, cancellationToken);
        string email = await _gitHubHttpService.GetUserEmail(accessToken, cancellationToken);
        return new UserRegistrationData(email);
    }
}
