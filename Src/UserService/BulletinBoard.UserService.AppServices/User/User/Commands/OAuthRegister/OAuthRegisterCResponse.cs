namespace BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister;

public class OAuthRegisterCResponse
{
    public string TokenType { get; init; }
    public string AccessToken { get; init; }
    public int ExpiresIn { get; init; }
    public string RefreshToken { get; init; }
}
