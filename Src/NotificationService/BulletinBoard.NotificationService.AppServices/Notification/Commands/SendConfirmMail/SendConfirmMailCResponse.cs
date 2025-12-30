namespace BulletinBoard.NotificationService.AppServices.Notification.Commands.SendConfirmMail;

public class SendConfirmMailCResponse
{
    public bool IsSucceed { get; init; }

    public SendConfirmMailCResponse(bool isSucceed = true)
    {
        IsSucceed = isSucceed;
    }
}
