using BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth;

namespace BulletinBoard.UserService.Infrastructure.Services.OAuth;

public class GitHubOAuthService : IOAuthService
{
    public Task<UserRegistrationData> GetRegistrationDataFromProviderAsync(string token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
