using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;


namespace BulletinBoard.UserService.AppServices.User.Helpers.Repository.UserRepository;

public interface IUserRepository
{
    public Task<IdentityUser?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken);
    public Task<List<IdentityUser>> GetCursorPaginatedUsersData(
        CursorData cursorData,
        int limit,
        CancellationToken cancellationToken);
}
