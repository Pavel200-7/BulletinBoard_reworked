using Microsoft.AspNetCore.Identity;


namespace BulletinBoard.UserService.AppServices.User.Repositiry;

public interface IUserRepository
{
    public Task<IdentityUser?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken);
}
