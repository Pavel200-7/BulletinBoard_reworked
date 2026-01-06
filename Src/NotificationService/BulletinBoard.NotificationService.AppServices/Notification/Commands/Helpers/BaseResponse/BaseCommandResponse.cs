namespace BulletinBoard.NotificationService.AppServices.Notification.Commands.Helpers.BaseResponse;

public class BaseCommandResponse
{
    public bool IsSucceed { get; init; }

    public BaseCommandResponse(bool isSucceed = true)
    {
        IsSucceed = isSucceed;
    }
}
