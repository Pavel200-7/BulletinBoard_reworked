namespace BulletinBoard.NotificationService.AppServices.User.Queries.Helpers.JWTGenerator;

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
