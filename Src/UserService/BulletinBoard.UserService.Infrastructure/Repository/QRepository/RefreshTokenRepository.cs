using BulletinBoard.NotificationService.AppServices.Common.IRepository;
using BulletinBoard.NotificationService.AppServices.User.Repositiry;
using BulletinBoard.NotificationService.Domain.Entityes;
using Microsoft.EntityFrameworkCore;


namespace BulletinBoard.NotificationService.Infrastructure.Repository.QRepository;

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

    public async Task<RefreshToken?> GetRefreshTokensByTokenStringAsync(string tokenString, CancellationToken cancellationToken)
    {
        return await _repository.GetAll()
            .FirstOrDefaultAsync(rt => rt.Token == tokenString, cancellationToken);
            
    }
}
