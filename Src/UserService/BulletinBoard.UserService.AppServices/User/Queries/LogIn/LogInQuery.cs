using MediatR;


namespace BulletinBoard.NotificationService.AppServices.User.Queries.LogIn;

/// <summary>
/// Авторизоваться.
/// </summary>
public class LogInQuery : IRequest<LogInQResponse>
{
    public string Email { get; init; }
    public string Password { get; init; }
}
