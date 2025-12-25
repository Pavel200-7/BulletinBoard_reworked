using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;
using BulletinBoard.UserService.AppServices.User.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Commands.AddRole;

public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, AddRoleCResponse>
{
    private readonly ILogger<AddRoleCommandHandler> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public AddRoleCommandHandler(ILogger<AddRoleCommandHandler> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<AddRoleCResponse> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            throw new NotFoundException("Пользователь с таким id не существует");
        }
        if (!Roles.IsRole(request.Role))
        {
            throw new BusinessRuleException(nameof(request.Role), "Такой роли не существует");
        }

        var result = await _userManager.AddToRoleAsync(user, request.Role);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        }

        return new AddRoleCResponse() { IsSucceed = result.Succeeded };
    }
}
