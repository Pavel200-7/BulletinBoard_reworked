namespace BulletinBoard.UserService.AppServices.User.Commands.Helpers.OAuth;

public interface IOAuthService
{
    /// <summary>
    /// Получить информацию о пользователе из провайдера по поступающему токену.
    /// </summary>
    /// <param name="provider">Имя провайдера.</param>
    /// <param name="token">Токен доступа выданный провайдером в редиректе.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Информация о пользователе для регистрации с частичными данными.</returns>
    public Task<UserRegistrationData> GetRegistrationDataFromProviderAsync(string provider, string token, CancellationToken cancellationToken);

    /// <summary>
    /// Сформировать URL для редиректа на страницу выдачи токена oAuth 2 соответствующего провайдера.
    /// </summary>
    /// <param name="provider">Имя провайдера.</param>
    /// <param name="state">Код для защиты от CSRF атак, передаваемый провайдеру для формирования своего редиректа.</param>
    /// <returns>URL для редиректа на провайдера.</returns>
    public string CreateToProviderRedirectURl(string provider, string state);
}
