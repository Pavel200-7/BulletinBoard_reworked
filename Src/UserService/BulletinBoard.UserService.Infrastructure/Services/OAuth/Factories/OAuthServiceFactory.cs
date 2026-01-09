using BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister.Helpers.OAuth;
using BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister.Helpers.OAuth.Factories;
using BulletinBoard.UserService.AppServices.User.User.Helpers.Enum;
using BulletinBoard.UserService.Infrastructure.Services.OAuth.GitHubOAuth;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.UserService.Infrastructure.Services.OAuth.Factories;

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
            OAuthProviders.GitHub => _serviceProvider.GetRequiredService<GitHubOAuthService>(),
            _ => throw new ArgumentException($"Неизвестный провайдер: {provider}")
        };
    }
}
