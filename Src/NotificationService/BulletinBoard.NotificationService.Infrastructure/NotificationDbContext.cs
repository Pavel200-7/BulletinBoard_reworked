using Microsoft.EntityFrameworkCore;


namespace BulletinBoard.NotificationService.Infrastructure;

public class NotificationDbContext : DbContext
{

    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) :
        base(options)
    { }
}
