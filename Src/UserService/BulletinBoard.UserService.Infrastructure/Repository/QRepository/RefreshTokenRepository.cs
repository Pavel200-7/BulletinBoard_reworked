using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using BulletinBoard.UserService.Domain.Entityes;
using Microsoft.EntityFrameworkCore;


namespace BulletinBoard.UserService.Infrastructure.Repository.QRepository;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private IQueryRepository<RefreshToken> _repository;

    public RefreshTokenRepository(IQueryRepository<RefreshToken> repository)
    {
        _repository = repository;
    }
    public async Task<List<RefreshToken>> GetRefreshTokensByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        return await _repository.GetAll()
            .Where(r => r.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}
