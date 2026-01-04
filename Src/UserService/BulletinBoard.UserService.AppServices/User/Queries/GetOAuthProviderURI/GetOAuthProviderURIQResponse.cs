namespace BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI;

public class GetOAuthProviderURIQResponse
{
    public string ProviderURI { get; init; }

    public GetOAuthProviderURIQResponse(string providerURI)
    {
        ProviderURI = providerURI;
    }
}
