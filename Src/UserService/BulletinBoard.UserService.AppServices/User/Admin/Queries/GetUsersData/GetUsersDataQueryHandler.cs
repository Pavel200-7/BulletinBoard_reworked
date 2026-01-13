using BulletinBoard.UserService.AppServices.User.Helpers.Repository.UserRepository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.UserService.AppServices.User.Admin.Queries.GetUsersData;

public class GetUsersDataQueryHandler : IRequestHandler<GetUsersDataQuery, GetUsersDataQResponse>
{
    private readonly ILogger<GetUsersDataQueryHandler> _logger;
    private readonly IUserRepository _qRepository;

    public GetUsersDataQueryHandler(
        ILogger<GetUsersDataQueryHandler> logger, 
        IUserRepository qRepository)
    {
        _logger = logger;
        _qRepository = qRepository;
    }

    public async Task<GetUsersDataQResponse> Handle(GetUsersDataQuery request, CancellationToken cancellationToken)
    {
        CursorData cursorData;
        if (string.IsNullOrEmpty(request.Cursor))
        {
            cursorData = new CursorData(request.Search);
        }
        else
        {
            cursorData = CursorData.Deserialize(request.Cursor);
        }

        return new GetUsersDataQResponse();

    }
}
