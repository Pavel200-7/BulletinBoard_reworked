using BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor;
using BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor.Factories;
using MediatR;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI;

public class GetOAuthProviderURIQueryHandler : IRequestHandler<GetOAuthProviderURIQuery, GetOAuthProviderURIQResponse>
{
    private readonly ILogger<GetOAuthProviderURIQueryHandler> _logger;
    private IURIConstructorFactory _uriConstructorFactory;

    public GetOAuthProviderURIQueryHandler(ILogger<GetOAuthProviderURIQueryHandler> logger, IURIConstructorFactory uriConstructorFactory)
    {
        _logger = logger;
        _uriConstructorFactory = uriConstructorFactory;
    }

    public Task<GetOAuthProviderURIQResponse> Handle(GetOAuthProviderURIQuery request, CancellationToken cancellationToken)
    {
        var IURIConstructor = _uriConstructorFactory.CreateConstructor(request.Provider);
        var urlData = new URIData() { State  = request.State };
        string providerOAuthURI = IURIConstructor.CreateURI(urlData);
        return Task.FromResult(
            new GetOAuthProviderURIQResponse(providerOAuthURI));
    }
}
