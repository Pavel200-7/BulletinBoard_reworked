using BulletinBoard.NotificationService.Domain.Entityes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace BulletinBoard.NotificationService.Infrastructure;

public class UserDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options) :
        base(options)
    { }
}
