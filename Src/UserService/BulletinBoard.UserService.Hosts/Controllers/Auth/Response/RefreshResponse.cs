namespace BulletinBoard.UserService.Hosts.Controllers.Auth.Response;

public class RefreshResponse
{
    public string TokenType { get; set; }
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
}
