namespace BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth.Factories;

public class OAuthServiceFactory : IOAuthServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public OAuthServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IOAuthService CreateOAuthService(string provider)
    {
        return provider.ToLower() switch
        {
            //OAuthProviders.GitHub => _serviceProvider.GetRequiredService<GitHubURIConstructor>(),
            _ => throw new ArgumentException($"Unknown provider: {provider}")
        };
    }
}
