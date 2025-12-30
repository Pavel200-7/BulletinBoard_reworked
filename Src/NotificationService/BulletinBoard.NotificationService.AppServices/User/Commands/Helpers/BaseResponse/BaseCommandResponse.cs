namespace BulletinBoard.NotificationService.AppServices.User.Commands.Helpers.BaseResponse;

public class BaseCommandResponse
{
    public bool IsSucceed { get; init; }

    public BaseCommandResponse(bool isSucceed = true)
    {
        IsSucceed = isSucceed;
    }
}
