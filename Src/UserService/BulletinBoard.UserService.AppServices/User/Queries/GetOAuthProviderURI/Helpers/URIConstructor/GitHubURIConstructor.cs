using BulletinBoard.UserService.AppServices.Common.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor;

public class GitHubURIConstructor : IURIConstructor
{
    private readonly ILogger<GitHubURIConstructor> _logger;
    private readonly GitHubOAuthSettings _settings;

    public GitHubURIConstructor(ILogger<GitHubURIConstructor> logger, IOptions<GitHubOAuthSettings> settings)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    public string CreateURI(URIData data)
    {
        return $"https://github.com/login/oauth/authorize?" +
            $"client_id={Uri.EscapeDataString(_settings.ClientId)}&" +
            $"redirect_uri={Uri.EscapeDataString(_settings.RedirectURI)}&" +
            $"state={Uri.EscapeDataString(data.State)}&" +
            $"allow_signup=false";
    }
}
