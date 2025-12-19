using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn.Helpers.JWTGenerator;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Queries.LogIn;

public class LogInQueryHandler : IRequestHandler<LogInQuery, LogInResponse>
{
    private ILogger<LogInQueryHandler> _logger;
    private IMapper _mapper;
    private UserManager<IdentityUser> _userManager;
    private SignInManager<IdentityUser> _signInManager;
    private IJWTGenerator _jWTGenerator;

    public LogInQueryHandler(
        ILogger<LogInQueryHandler> logger, 
        IMapper mapper, 
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IJWTGenerator jWTGenerator
        )
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _jWTGenerator = jWTGenerator;
    }

    public async Task<LogInResponse> Handle(LogInQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new NotFoundException("Пользователь с такой почтой не обнаружен.");
        }

        bool isPersistent = false;
        bool lockoutOnFailure = false;
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, isPersistent, lockoutOnFailure);
        if (!result.Succeeded) 
        {
            throw new BusinessRuleException("Password", "Неверный пароль.");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var claimsData = new ClaimsData()
        {
            UserId = user.Id,
            Email = user.Email!,
            Roles = userRoles.ToList()
        };
        var tokenData = _jWTGenerator.GenerateToken(claimsData);

        var refreshToken = await _userManager.GenerateUserTokenAsync(
           user,
           "RefreshTokenProvider", 
           "Refresh");             

        return new LogInResponse()
        {
            TokenType = tokenData.TokenType,    
            AccessToken = tokenData.AccessToken,
            ExpiresIn = tokenData.ExpiresIn,
            RefreshToken = refreshToken,
        };
    }
}
