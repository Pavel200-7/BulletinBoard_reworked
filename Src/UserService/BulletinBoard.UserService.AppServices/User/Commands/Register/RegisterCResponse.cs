using BulletinBoard.UserService.AppServices.User.Commands.Helpers.BaseResponse;

namespace BulletinBoard.UserService.AppServices.User.Commands.Register;

public class RegisterCResponse : BaseCommandResponse
{
    public string TokenType { get; init; }
    public string AccessToken { get; init; }
    public int ExpiresIn { get; init; }
    public string RefreshToken { get; init; }
}
