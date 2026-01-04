using MediatR;


namespace BulletinBoard.NotificationService.AppServices.Notification.Commands.SendConfirmMail;

/// <summary>
/// Отправить письмо для подтверждения почты.
/// </summary>
public class SendConfirmMailCommand : IRequest<SendConfirmMailCResponse>
{
    public string Id { get; init; }
    public string Token { get; init; }
}
