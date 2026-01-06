using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace BulletinBoard.UserService.Infrastructure;

public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
{
    public UserDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Host=postgre_user_db;Port=5432;Database=userdb;Username=postgres;Password=password";

        var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
        optionsBuilder.UseNpgsql(connectionString,
            b => b.MigrationsAssembly("BulletinBoard.UserService.Infrastructure"));

        return new UserDbContext(optionsBuilder.Options);
    }
}