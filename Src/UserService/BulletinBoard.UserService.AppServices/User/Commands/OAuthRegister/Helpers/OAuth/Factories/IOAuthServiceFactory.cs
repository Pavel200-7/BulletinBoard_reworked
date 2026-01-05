namespace BulletinBoard.NotificationService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth.Factories;

public interface IOAuthServiceFactory
{
    public IOAuthService CreateOAuthService(string provider);
}
