using BulletinBoard.UserService.AppServices.User.User.Helpers.Enum;
using BulletinBoard.UserService.AppServices.User.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.UserService.AppServices.User.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor.Factories;

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
