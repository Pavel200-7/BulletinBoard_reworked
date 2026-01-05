using Microsoft.AspNetCore.Identity;


namespace BulletinBoard.NotificationService.AppServices.User.Repositiry;

public interface IUserRepository
{
    /// <summary>
    /// Найти пользователя по телефону.
    /// </summary>
    /// <param name="phoneNumber">Телефон пользователя</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Пользователь</returns>
    public Task<IdentityUser?> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken);
}
