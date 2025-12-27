using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar.Registrar;

public static class DbContextRegistrar
{
    public static IServiceCollection RegistrarDbContext(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<NotificationDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("BulletinBoard.NotificationService.Infrastructure")
            )
            .EnableSensitiveDataLogging() 
            .EnableDetailedErrors() 
            .LogTo(Console.WriteLine, 
                new[] {
                    DbLoggerCategory.Database.Transaction.Name, 
                    DbLoggerCategory.Database.Command.Name,     
                    DbLoggerCategory.Database.Connection.Name   
                },
                LogLevel.Information,
                DbContextLoggerOptions.SingleLine); ;
            });
        return services;
    }
}
