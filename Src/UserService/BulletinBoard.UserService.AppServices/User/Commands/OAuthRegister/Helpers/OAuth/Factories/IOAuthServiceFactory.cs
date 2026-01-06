using BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth;

namespace BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth.Factories;

public interface IOAuthServiceFactory
{
    public IOAuthService CreateOAuthService(string provider);
}
