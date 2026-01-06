namespace BulletinBoard.UserService.AppServices.User.Commands.Helpers.BaseResponse;

/// <summary>
/// Базовый ответ команды.
/// </summary>
public class BaseCommandResponse
{
    public bool IsSucceed { get; init; }

    public BaseCommandResponse(bool isSucceed = true)
    {
        IsSucceed = isSucceed;
    }
}
