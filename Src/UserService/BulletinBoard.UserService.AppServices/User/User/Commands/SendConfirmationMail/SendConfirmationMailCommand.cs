using MediatR;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.SendConfirmationMail;

public class SendConfirmationMailCommand : IRequest<SendConfirmationMailCResponse>
{
    /// <summary>
    /// Id пользователя.
    /// </summary>
    public string Id { get; set; }
}
