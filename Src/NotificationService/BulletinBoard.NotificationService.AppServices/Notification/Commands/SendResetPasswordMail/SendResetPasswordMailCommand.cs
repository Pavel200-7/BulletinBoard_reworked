using MediatR;


namespace BulletinBoard.NotificationService.AppServices.Notification.Commands.SendResetPasswordMail;

public class SendResetPasswordMailCommand : IRequest<SendResetPasswordMailCResponse>
{
    public string Id { get; init; }
    public string Token { get; init; }
}
