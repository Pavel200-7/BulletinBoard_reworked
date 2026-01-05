using BulletinBoard.NotificationService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth;
using BulletinBoard.NotificationService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth.Factories;
using BulletinBoard.NotificationService.AppServices.User.Enum;
using BulletinBoard.NotificationService.Infrastructure.Services.OAuth.GitHubOAuth;
using Microsoft.Extensions.DependencyInjection;

namespace BulletinBoard.NotificationService.Infrastructure.Services.OAuth.Factories;

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
