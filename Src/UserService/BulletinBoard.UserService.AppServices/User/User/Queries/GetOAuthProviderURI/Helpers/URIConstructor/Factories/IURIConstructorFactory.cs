using BulletinBoard.UserService.AppServices.User.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor;

namespace BulletinBoard.UserService.AppServices.User.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor.Factories;

public interface IURIConstructorFactory
{
    public IURIConstructor CreateConstructor(string provider);
}
