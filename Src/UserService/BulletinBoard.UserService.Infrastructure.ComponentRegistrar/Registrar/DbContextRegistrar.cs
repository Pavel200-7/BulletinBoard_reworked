using BulletinBoard.Infrastructure.DataAccess.User.WriteContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.Infrastructure.ComponentRegistrar.Registrar;

public static class DbContextRegistrar
{
    public static IServiceCollection RegistrarDbContext(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<UserContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("BulletinBoard.UserService.Infrastructure.DataAccess")
            );
        });
        return services;
    }
}
