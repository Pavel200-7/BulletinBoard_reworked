using BulletinBoard.NotificationService.Domain.Entities.Base;

namespace BulletinBoard.NotificationService.Domain.Entities;

public class AppUser : BaseEntity
{
    public string Email { get; set; }


    public AppUser(string userId, string email)
    {
        Id = Guid.Parse(userId);
        Email = email;
    }

    public AppUser()
    {
    }
}
