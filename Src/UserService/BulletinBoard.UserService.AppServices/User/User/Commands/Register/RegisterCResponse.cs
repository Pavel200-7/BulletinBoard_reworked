using BulletinBoard.UserService.AppServices.Common.Helpers.BaseCommandResponse;

namespace BulletinBoard.UserService.AppServices.User.User.Commands.Register;

public class RegisterCResponse : BaseCResponse
{
    public string TokenType { get; init; }
    public string AccessToken { get; init; }
    public int ExpiresIn { get; init; }
    public string RefreshToken { get; init; }
}
