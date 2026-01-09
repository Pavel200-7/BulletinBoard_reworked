namespace BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister.Helpers.OAuth;

public interface IOAuthService
{
    /// <summary>
    /// Получить информацию о пользователе из провайдера по поступающему коду.
    /// </summary>
    /// <param name="provider">Имя провайдера</param>
    /// <param name="code">Код выданный провайдером в редиректе.</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Информация о пользователе для регистрации с частичными данными</returns>
    public Task<UserRegistrationData> GetRegistrationDataFromProviderAsync(string code, CancellationToken cancellationToken);
}
