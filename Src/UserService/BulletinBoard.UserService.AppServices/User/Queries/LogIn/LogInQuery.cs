using MediatR;


namespace BulletinBoard.UserService.AppServices.User.Queries.LogIn;

public class LogInQuery : IRequest<LogInResponse>
{
    public string Email { get; init; }
    public string Password { get; init; }
}
