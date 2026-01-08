using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace BulletinBoard.UserService.AppServices.User.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordCResponse>
{
    private readonly ILogger<ResetPasswordCommandHandler> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public ResetPasswordCommandHandler(
        ILogger<ResetPasswordCommandHandler> logger, 
        UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<ResetPasswordCResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new NotFoundException("Пользователь с такой почтой не обнаружен.");
        }

        var token = Base64UrlEncoder.Decode(request.Token);
        var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        }

        return new ResetPasswordCResponse();
    }
}
