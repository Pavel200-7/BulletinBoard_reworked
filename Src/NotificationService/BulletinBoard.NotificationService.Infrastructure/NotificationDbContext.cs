using BulletinBoard.NotificationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace BulletinBoard.NotificationService.Infrastructure;

public class NotificationDbContext : DbContext
{
    public DbSet<AppUser> Users { get; set; }

    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) :
        base(options)
    { }
}
