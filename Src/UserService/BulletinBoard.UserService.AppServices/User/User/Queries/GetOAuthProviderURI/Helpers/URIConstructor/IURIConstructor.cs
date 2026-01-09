namespace BulletinBoard.UserService.AppServices.User.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor;

public interface IURIConstructor
{
    /// <summary>
    /// Сформировать URL для редиректа на страницу выдачи токена oAuth 2 соответствующего провайдера.
    /// </summary>
    /// <param name="data">Информация для URL.</param>
    /// <returns>URL для редиректа на провайдера</returns>
    public string CreateURI(URIData data);
}
