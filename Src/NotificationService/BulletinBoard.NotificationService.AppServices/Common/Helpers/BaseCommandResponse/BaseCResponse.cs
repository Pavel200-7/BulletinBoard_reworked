namespace BulletinBoard.NotificationService.AppServices.Common.Helpers.BaseCommandResponse;

/// <summary>
/// Базовый ответ команды.
/// </summary>
public class BaseCResponse
{
    public bool IsSucceed { get; init; }

    public BaseCResponse(bool isSucceed = true)
    {
        IsSucceed = isSucceed;
    }
}
