using MediatR;


namespace BulletinBoard.UserService.AppServices.User.Queries.LogIn;

public class LogInQuery : IRequest<LogInQResponse>
{
    public string Email { get; init; }
    public string Password { get; init; }
}
