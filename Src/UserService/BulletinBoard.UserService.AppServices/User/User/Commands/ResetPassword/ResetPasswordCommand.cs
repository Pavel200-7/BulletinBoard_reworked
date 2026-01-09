using MediatR;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<ResetPasswordCResponse>
{
    public string Email { get; init; }
    public string Token { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
}
