using MediatR;

namespace BulletinBoard.UserService.AppServices.User.Queries.Refresh;

public class RefreshQuery : IRequest<RefreshQResponse>
{
    public string RefreshToken { get; init; }
}
