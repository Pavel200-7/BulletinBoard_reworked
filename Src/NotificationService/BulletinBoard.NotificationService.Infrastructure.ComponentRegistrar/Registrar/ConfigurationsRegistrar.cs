using BulletinBoard.NotificationService.AppServices.Common.Configurations;
using BulletinBoard.NotificationService.Infrastructure.Common.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;

public static class ConfigurationsRegistrar
{
    public static IServiceCollection RegistrarConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(options => configuration.GetSection("EmailSettings").Bind(options));
        services.Configure<APIGatewayData>(options => configuration.GetSection("APIGateway").Bind(options));

        return services;
    }
}
