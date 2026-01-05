using MediatR;


namespace BulletinBoard.NotificationService.AppServices.User.Commands.SendConfirmationMail;

public class SendConfirmationMailCommand : IRequest<SendConfirmationMailCResponse>
{
    /// <summary>
    /// Id пользователя.
    /// </summary>
    public string Id { get; set; }
}
