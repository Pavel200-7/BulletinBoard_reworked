using BulletinBoard.UserService.Domain.Entities;


namespace BulletinBoard.UserService.AppServices.User.Repositiry;

public interface IRefreshTokenRepository
{
    /// <summary>
    /// Получить токен обновления пользователя по id.
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="cancellationToken">Токен обновления</param>
    /// <returns>Токен обновления</returns>
    public Task<List<RefreshToken>> GetRefreshTokensByUserIdAsync(string userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить токен обновления пользователя по его строке.
    /// </summary>
    /// <param name="tokenString">Строка токена обновления</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Токен обновления</returns>
    public Task<RefreshToken?> GetRefreshTokensByTokenStringAsync(string tokenString, CancellationToken cancellationToken);
}
