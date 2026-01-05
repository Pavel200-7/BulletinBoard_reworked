using BulletinBoard.NotificationService.AppServices.Common.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar.Registrar;

public static class ConfigurationsRegistrar
{
    public static IServiceCollection RegistrarConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JWT"));
        services.Configure<RefreshTokenSettings>(configuration.GetSection("Refresh"));

        services.Configure<GitHubOAuthSettings>(configuration.GetSection("GitHub"));


        return services;
    }
}
