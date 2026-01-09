using MediatR;


namespace BulletinBoard.UserService.AppServices.User.User.Queries.GetOAuthProviderURI;

public class GetOAuthProviderURIQuery : IRequest<GetOAuthProviderURIQResponse>
{
    public string Provider { get; init; }
    public string State { get; init; }

    public GetOAuthProviderURIQuery(string provider, string state)
    {
        Provider = provider;
        State = state;
    }
}
