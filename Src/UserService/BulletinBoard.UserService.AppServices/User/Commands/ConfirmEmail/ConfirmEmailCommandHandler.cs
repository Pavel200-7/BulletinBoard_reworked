using BulletinBoard.NotificationService.AppServices.Common.Exceptions;
using BulletinBoard.NotificationService.AppServices.Common.Exceptions.Common.FieldFailures;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace BulletinBoard.NotificationService.AppServices.User.Commands.ConfirmEmail;

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
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            throw new NotFoundException("Пользователь с таким id не существует");
        }
        var token = Base64UrlEncoder.Decode(request.Token);
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        }
        _logger.LogInformation("Пользователь с id {0} подтвердил почту.", user.Id);

        return new ConfirmEmailCResponse();
    }
}
