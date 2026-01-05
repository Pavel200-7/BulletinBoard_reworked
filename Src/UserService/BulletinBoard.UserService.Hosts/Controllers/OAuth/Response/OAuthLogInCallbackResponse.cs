namespace BulletinBoard.NotificationService.Hosts.Controllers.OAuth.Response;

public class OAuthLogInCallbackResponse
{
    public string TokenType { get; init; }
    public string AccessToken { get; init; }
    public int ExpiresIn { get; init; }
    public string RefreshToken { get; init; }
}
