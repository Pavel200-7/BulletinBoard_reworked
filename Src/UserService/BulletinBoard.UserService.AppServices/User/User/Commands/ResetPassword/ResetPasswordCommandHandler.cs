using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base.FieldFailures;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.ResetPassword;

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
        var user = await _userManager.FindByEmailAsync(request.Email)
            .ThrowNotFoundIfNull("Пользователь с такой почтой не обнаружен.");

        var token = Base64UrlEncoder.Decode(request.Token);
        var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
        result.Succeeded
            .ThrowBusinessRuleIfFalse(FieldFailuresConverter.FromIdentityErrors(result.Errors));

        return new ResetPasswordCResponse();
    }
}
