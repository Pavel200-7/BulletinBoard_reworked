using MediatR;


namespace BulletinBoard.UserService.AppServices.User.Admin.Queries.GetUsersData;

public class GetUsersDataQuery : IRequest<GetUsersDataQResponse>
{
    public string Search { get; init; }
    public string Cursor { get; init; }
    public int Limit { get; init; } = 20;
}
