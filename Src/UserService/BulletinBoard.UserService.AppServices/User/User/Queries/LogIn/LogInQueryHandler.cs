using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Behaviors.Transaction;
using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base.FieldFailures;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using BulletinBoard.UserService.AppServices.User.User.Helpers.JWT;
using BulletinBoard.UserService.AppServices.User.User.Helpers.RefreshT;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.User.Queries.LogIn;

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
        var user = await _userManager.FindByEmailAsync(request.Email)
            .ThrowNotFoundIfNull("Пользователь с такой почтой не обнаружен.");

        bool isPersistent = false;
        bool lockoutOnFailure = false;
        var result = await _signInManager.PasswordSignInAsync(user, request.Password, isPersistent, lockoutOnFailure);
        result.Succeeded
            .ThrowBusinessRuleIfFalse(FieldFailuresConverter.FromSingleFieldError("Password", "Неверный пароль."));

        var tokenData = await _jWTProvider.GenerateTokenAsync(user.Id, cancellationToken);
        var refreshToken = await _refreshTProvider.GenerateTokenAsync(user.Id, cancellationToken);
        _logger.LogInformation("Пользователь с id {0} вошел в систему.", user.Id);
        return new LogInQResponse()
        {
            TokenType = tokenData.TokenType,    
            AccessToken = tokenData.AccessToken,
            ExpiresIn = tokenData.ExpiresIn,
            RefreshToken = refreshToken,
        };
    }
}
