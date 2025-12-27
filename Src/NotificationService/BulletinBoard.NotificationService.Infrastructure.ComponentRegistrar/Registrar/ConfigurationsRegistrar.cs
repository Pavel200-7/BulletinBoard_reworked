using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;

public static class ConfigurationsRegistrar
{
    public static IServiceCollection RegistrarConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
