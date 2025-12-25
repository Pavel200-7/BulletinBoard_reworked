namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;

public interface IJWTProvider
{
    public Task<TokenData> GenerateTokenAsync(string userId, CancellationToken cancellationToken);
}
