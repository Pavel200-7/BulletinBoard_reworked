using MediatR;


namespace BulletinBoard.UserService.AppServices.User.User.Queries.Refresh;

/// <summary>
/// Обновить данные авторизации по токену обновления.
/// </summary>
public class RefreshQuery : IRequest<RefreshQResponse>
{
    public string RefreshToken { get; init; }
}
