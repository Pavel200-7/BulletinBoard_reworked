using MediatR;


namespace BulletinBoard.UserService.AppServices.User.Queries.Refresh;

public class RefreshQueryHandler : IRequestHandler<RefreshQuery, RefreshQResponse>
{
    public async Task<RefreshQResponse> Handle(RefreshQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
