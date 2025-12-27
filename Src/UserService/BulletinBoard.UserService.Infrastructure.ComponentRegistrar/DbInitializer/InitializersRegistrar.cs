using BulletinBoard.UserService.Infrastructure.ComponentRegistrar.DbInitializer;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.Infrastructure.ComponentRegistrar.DbInitializer;

public static class InitializersRegistrar
{
    public static IServiceCollection RegistrarInitializers(this IServiceCollection services)
    {
        services.AddAsyncInitializer<DbInitializer>();
        services.AddAsyncInitializer<RolesInitializer>();

        return services;
    }
}
