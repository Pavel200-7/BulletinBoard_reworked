using BulletinBoard.UserService.AppServices.Common.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;

public static class ConfigurationsRegistrar
{
    public static IServiceCollection RegistrarConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JWT"));
        services.Configure<RefreshTokenOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromDays(7);
        });

        return services;
    }
}
