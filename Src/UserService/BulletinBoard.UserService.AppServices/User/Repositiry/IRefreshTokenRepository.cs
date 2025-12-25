using BulletinBoard.UserService.Domain.Entityes;


namespace BulletinBoard.UserService.AppServices.User.Repositiry;

public interface IRefreshTokenRepository
{
    public Task<List<RefreshToken>> GetRefreshTokensByUserIdAsync(string userId, CancellationToken cancellationToken);
    public Task<RefreshToken?> GetRefreshTokensByTokenStringAsync(string tokenString, CancellationToken cancellationToken);
}
