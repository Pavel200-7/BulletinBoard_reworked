namespace BulletinBoard.UserService.AppServices.User.User.Helpers.JWT;

public interface IJWTProvider
{
    /// <summary>
    /// Сгенерировать JWT.
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Информация JWT</returns>
    public Task<TokenData> GenerateTokenAsync(string userId, CancellationToken cancellationToken);
}
