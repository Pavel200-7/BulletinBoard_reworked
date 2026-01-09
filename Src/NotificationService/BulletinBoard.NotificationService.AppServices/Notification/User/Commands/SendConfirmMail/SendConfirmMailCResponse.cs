using BulletinBoard.NotificationService.AppServices.Common.Helpers.BaseCommandResponse;


namespace BulletinBoard.NotificationService.AppServices.Notification.User.Commands.SendConfirmMail;

public class SendConfirmMailCResponse : BaseCResponse
{
    public bool IsSucceed { get; init; }

    public SendConfirmMailCResponse(bool isSucceed = true)
    {
        IsSucceed = isSucceed;
    }
}
