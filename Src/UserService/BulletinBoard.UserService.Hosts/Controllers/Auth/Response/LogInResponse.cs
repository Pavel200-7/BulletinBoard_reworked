namespace BulletinBoard.UserService.Hosts.Controllers.Auth.Response;

public class LogInResponse
{
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
}
