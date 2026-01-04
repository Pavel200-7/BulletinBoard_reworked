using BulletinBoard.UserService.AppServices.User.Enum;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor.Factories;

public class URIConstructorFactory : IURIConstructorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public URIConstructorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IURIConstructor CreateConstructor(string provider)
    {
        return provider.ToLower() switch
        {
            OAuthProviders.GitHub => _serviceProvider.GetRequiredService<GitHubURIConstructor>(),
            _ => throw new ArgumentException($"Неизвестный провайдер: {provider}")
        };
    }
}
