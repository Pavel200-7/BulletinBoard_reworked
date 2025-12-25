namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;

public interface IJWTProvider
{
    public Task<TokenData> GenerateToken(string userId, CancellationToken cancellationToken);
}
