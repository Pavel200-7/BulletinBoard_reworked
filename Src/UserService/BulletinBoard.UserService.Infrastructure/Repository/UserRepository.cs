using BulletinBoard.UserService.AppServices.User.Repositiry;
using BulletinBoard.UserService.Infrastructure.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.UserService.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private IRepository<IdentityUser, UserDbContext> _repository;

    public UserRepository(IRepository<IdentityUser, UserDbContext> repository)
    {
        _repository = repository;
    }

    public async Task<IdentityUser?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return await _repository.GetAll().FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }
}
