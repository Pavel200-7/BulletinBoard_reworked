using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;
using BulletinBoard.UserService.AppServices.User.Commands.ChangePhone;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Commands.ChangeUserName;

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
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is not null)
        {
            throw new BusinessRuleException(nameof(request.UserName), "Данное имя уже занято.");
        }

        user = await _userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            throw new NotFoundException("Пользователь с таким id не существует");
        }

        var result = await _userManager.SetUserNameAsync(user, request.UserName);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        }

        return new ChangeUserNameCResponse();
    }
}
