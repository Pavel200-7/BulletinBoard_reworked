using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Behaviors.TransactionBehavior;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshT;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Queries.LogIn;

[Transaction]
public class LogInQueryHandler : IRequestHandler<LogInQuery, LogInQResponse>
{
    private readonly ILogger<LogInQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IJWTProvider _jWTProvider;
    private readonly IRefreshTokenProvider _refreshTProvider;

    public LogInQueryHandler(
        ILogger<LogInQueryHandler> logger, 
        IMapper mapper, 
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IJWTProvider jWTProvider,
        IRefreshTokenProvider refreshTokenProvider
        )
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _jWTProvider = jWTProvider;
        _refreshTProvider = refreshTokenProvider;
    }

    public async Task<LogInQResponse> Handle(LogInQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new NotFoundException("Пользователь с такой почтой не обнаружен.");
        }

        bool isPersistent = false;
        bool lockoutOnFailure = false;
        var result = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent, lockoutOnFailure);
        if (!result.Succeeded) 
        {
            throw new BusinessRuleException("Password", "Неверный пароль.");
        }

        var tokenData = await _jWTProvider.GenerateToken(user.Id, cancellationToken);
        var refreshToken = await _refreshTProvider.GenerateRefreshTokenAsync(user.Id, cancellationToken);           

        return new LogInQResponse()
        {
            TokenType = tokenData.TokenType,    
            AccessToken = tokenData.AccessToken,
            ExpiresIn = tokenData.ExpiresIn,
            RefreshToken = refreshToken,
        };
    }
}
