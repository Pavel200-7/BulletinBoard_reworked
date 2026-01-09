using MediatR;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.SendResetPasswordMail;

public class SendResetPasswordMailCommand : IRequest<SendResetPasswordMailCResponse>
{
    public string Email { get; init; }
}
