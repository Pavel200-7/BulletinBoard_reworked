using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BulletinBoard.UserService.Infrastructure.Repository.QRepository;

public class UserRepository : IUserRepository
{
    private IQueryRepository<IdentityUser> _repository;

    public UserRepository(IQueryRepository<IdentityUser> repository)
    {
        _repository = repository;
    }

    public async Task<IdentityUser?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken)
    {
        return await _repository.GetAll().FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }
}
