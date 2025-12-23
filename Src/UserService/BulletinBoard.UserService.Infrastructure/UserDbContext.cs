using BulletinBoard.UserService.Domain.Entityes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace BulletinBoard.UserService.Infrastructure;

public class UserDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options) :
        base(options)
    { }
}
