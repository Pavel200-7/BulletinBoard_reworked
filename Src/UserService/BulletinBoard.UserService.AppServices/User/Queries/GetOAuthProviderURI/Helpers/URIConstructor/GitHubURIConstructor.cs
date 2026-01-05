using BulletinBoard.NotificationService.AppServices.Common.Configurations;
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
        string scopeString = GetDefaultScopesString();
        return $"https://github.com/login/oauth/authorize?" +
            $"client_id={Uri.EscapeDataString(_settings.ClientId)}&" +
            $"redirect_uri={Uri.EscapeDataString(_settings.RedirectURI)}&" +
            $"scope={Uri.EscapeDataString(scopeString)}&" +
            $"state={Uri.EscapeDataString(data.State)}&" +
            $"allow_signup=false";
    }

    private string GetDefaultScopesString()
    {
        var scopes = new List<string>()
        {
            "read:user",    
            "user:email"    
        };
        return string.Join(" ", scopes);
    }
}
