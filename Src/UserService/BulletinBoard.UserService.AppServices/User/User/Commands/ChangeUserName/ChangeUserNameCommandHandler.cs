using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base.FieldFailures;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.ChangeUserName;

public class ChangeUserNameCommandHandler : IRequestHandler<ChangeUserNameCommand, ChangeUserNameCResponse>
{
    private readonly ILogger<ChangeUserNameCommandHandler> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public ChangeUserNameCommandHandler(
        ILogger<ChangeUserNameCommandHandler> logger,
        UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }
    public async Task<ChangeUserNameCResponse> Handle(ChangeUserNameCommand request, CancellationToken cancellationToken)
    {
        await _userManager.FindByNameAsync(request.UserName)
            .ThrowBusinessRuleIfNotNull(FieldFailuresConverter.FromSingleFieldError(nameof(request.UserName), "Данное имя уже занято."));
        var user = await _userManager.FindByIdAsync(request.Id)
            .ThrowNotFoundIfNull("Пользователь с таким id не существует");
        var result = await _userManager.SetUserNameAsync(user, request.UserName);
        result.Succeeded
            .ThrowBusinessRuleIfFalse(FieldFailuresConverter.FromIdentityErrors(result.Errors));

        return new ChangeUserNameCResponse();
    }
}
