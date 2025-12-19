namespace BulletinBoard.UserService.AppServices.User.Queries.LogIn.Helpers.JWTGenerator;

public interface IJWTGenerator
{
    public TokenData GenerateToken(ClaimsData claimsData);
}
