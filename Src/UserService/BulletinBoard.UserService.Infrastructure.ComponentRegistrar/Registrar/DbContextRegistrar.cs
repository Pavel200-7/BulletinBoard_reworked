using BulletinBoard.Infrastructure.DataAccess.User.WriteContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


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
            )
            .EnableSensitiveDataLogging() // Опционально (логирует параметры)
            .EnableDetailedErrors() // Подробные ошибки
            .LogTo(Console.WriteLine, // Куда писать логи
                new[] {
                    DbLoggerCategory.Database.Transaction.Name, // Транзакции
                    DbLoggerCategory.Database.Command.Name,     // SQL команды
                    DbLoggerCategory.Database.Connection.Name   // Подключения
                },
                LogLevel.Information,
                DbContextLoggerOptions.SingleLine); ;
            });
        return services;
    }
}
