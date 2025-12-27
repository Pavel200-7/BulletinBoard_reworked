using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace BulletinBoard.NotificationService.Infrastructure;

public class NotificationDbContextFactory : IDesignTimeDbContextFactory<NotificationDbContext>
{
    public NotificationDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Host=postgre_notification_db;Port=5432;Database=userdb;Username=postgres;Password=password";

        var optionsBuilder = new DbContextOptionsBuilder<NotificationDbContext>();
        optionsBuilder.UseNpgsql(connectionString,
            b => b.MigrationsAssembly("BulletinBoard.NotificationService.Infrastructure"));

        return new NotificationDbContext(optionsBuilder.Options);
    }
}