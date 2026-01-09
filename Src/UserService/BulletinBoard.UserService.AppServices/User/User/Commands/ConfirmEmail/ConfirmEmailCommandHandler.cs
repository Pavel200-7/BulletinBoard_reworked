using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base.FieldFailures;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ConfirmEmailCResponse>
{
    private readonly ILogger<ConfirmEmailCommandHandler> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public ConfirmEmailCommandHandler(ILogger<ConfirmEmailCommandHandler> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<ConfirmEmailCResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId)
            .ThrowNotFoundIfNull("Пользователь с таким id не существует");

        var token = Base64UrlEncoder.Decode(request.Token);
        var result = await _userManager.ConfirmEmailAsync(user, token);
        result.Succeeded
            .ThrowBusinessRuleIfFalse(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        _logger.LogInformation("Пользователь с id {0} подтвердил почту.", user.Id);

        return new ConfirmEmailCResponse();
    }
}
