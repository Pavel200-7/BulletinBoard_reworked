namespace BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth;

public interface IOAuthService
{
    /// <summary>
    /// Получить информацию о пользователе из провайдера по поступающему токену.
    /// </summary>
    /// <param name="provider">Имя провайдера</param>
    /// <param name="token">Токен доступа выданный провайдером в редиректе</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация о пользователе для регистрации с частичными данными</returns>
    public Task<UserRegistrationData> GetRegistrationDataFromProviderAsync(string token, CancellationToken cancellationToken);
}
