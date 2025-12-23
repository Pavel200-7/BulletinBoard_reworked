namespace BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;

public class TokenData
{
    public string TokenType { get; init; }
    public string AccessToken { get; init; }
    public int ExpiresIn { get; init; }
}
