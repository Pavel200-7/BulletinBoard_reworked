using MediatR;


namespace BulletinBoard.NotificationService.AppServices.User.Queries.Refresh;

/// <summary>
/// Обновить данные авторизации по токену обновления.
/// </summary>
public class RefreshQuery : IRequest<RefreshQResponse>
{
    public string RefreshToken { get; init; }
}
