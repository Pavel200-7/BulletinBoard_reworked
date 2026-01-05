namespace BulletinBoard.NotificationService.AppServices.User.Queries.Helpers.RefreshT;

public interface IRefreshTokenProvider
{
    /// <summary>
    /// Сгенерировать токен обновления.
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Строка токена обновления</returns>
    public Task<string> GenerateTokenAsync(string userId, CancellationToken cancellationToken);
}
