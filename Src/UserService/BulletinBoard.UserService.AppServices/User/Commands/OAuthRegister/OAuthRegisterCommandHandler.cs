using AutoMapper;
using BulletinBoard.NotificationService.AppServices.User.Commands.Helpers.RegisterCommandHandler;
using BulletinBoard.NotificationService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth.Factories;
using BulletinBoard.NotificationService.AppServices.User.Queries.Helpers.JWTGenerator;
using BulletinBoard.NotificationService.AppServices.User.Queries.Helpers.RefreshT;
using BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister;

public class OAuthRegisterCommandHandler : BaseRegisterCommandHandler, 
    IRequestHandler<OAuthRegisterCommand, OAuthRegisterCResponse>
{
    private readonly ILogger<OAuthRegisterCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IOAuthServiceFactory _oauthServiceFactory;
    private readonly IJWTProvider _jWTProvider;
    private readonly IRefreshTokenProvider _refreshTProvider;
    private readonly IPublishEndpoint _publishEndpoint;

    public OAuthRegisterCommandHandler(
        ILogger<OAuthRegisterCommandHandler> logger, 
        IMapper mapper,
        UserManager<IdentityUser> userManager,
        IOAuthServiceFactory oauthServiceFactory,
        IJWTProvider jWTProvider,
        IRefreshTokenProvider refreshTokenProvider,
        IPublishEndpoint publishEndpoint) : base(logger, mapper, userManager, publishEndpoint)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _oauthServiceFactory = oauthServiceFactory;
        _jWTProvider = jWTProvider;
        _refreshTProvider = refreshTokenProvider;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<OAuthRegisterCResponse> Handle(OAuthRegisterCommand request, CancellationToken cancellationToken)
    {
        var oauthService = _oauthServiceFactory.CreateOAuthService(request.Provider);
        var userData = await oauthService.GetRegistrationDataFromProviderAsync(request.Code, cancellationToken);
        
        IdentityUser? user = await _userManager.FindByEmailAsync(userData.Email);
        if (user is not null)
        {
            return await LoginAsync(user.Id, cancellationToken);
        }

        var partialUser = new PartialUser(userData.Email);
        user = _mapper.Map<IdentityUser>(partialUser);
        await RegisterAsync(user, partialUser.Password, cancellationToken);
        
        return await LoginAsync(user.Id, cancellationToken);
    }

    private async Task<OAuthRegisterCResponse> LoginAsync(string userId, CancellationToken cancellationToken)
    {
        var tokenData = await _jWTProvider.GenerateTokenAsync(userId, cancellationToken);
        var refreshToken = await _refreshTProvider.GenerateTokenAsync(userId, cancellationToken);
        return new OAuthRegisterCResponse()
        {
            TokenType = tokenData.TokenType,
            AccessToken = tokenData.AccessToken,
            ExpiresIn = tokenData.ExpiresIn,
            RefreshToken = refreshToken,
        };
    }
}
