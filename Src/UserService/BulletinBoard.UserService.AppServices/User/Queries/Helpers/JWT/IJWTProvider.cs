namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;

public interface IJWTProvider
{
    public TokenData GenerateToken(ClaimsData claimsData);
}
