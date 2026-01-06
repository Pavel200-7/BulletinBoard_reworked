namespace BulletinBoard.UserService.AppServices.User.Queries.Refresh;

public class RefreshQResponse
{
    public string TokenType { get; init; }
    public string AccessToken { get; init; }
    public int ExpiresIn { get; init; }
    public string RefreshToken { get; init; }
}
