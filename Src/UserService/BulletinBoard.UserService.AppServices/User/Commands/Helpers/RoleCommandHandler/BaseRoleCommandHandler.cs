using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Commands.AddRole;
using BulletinBoard.UserService.AppServices.User.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Commands.Helpers.RoleCommandHandler;

public abstract class BaseRoleCommandHandler
{
    protected readonly ILogger<AddRoleCommandHandler> _logger;
    protected readonly UserManager<IdentityUser> _userManager;

    protected BaseRoleCommandHandler(ILogger<AddRoleCommandHandler> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IdentityUser> ValidateCommandAndGetUser(string userId, string role, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            throw new NotFoundException("Пользователь с таким id не существует");
        }

        if (!Roles.IsRole(role))
        {
            throw new BusinessRuleException("Role", "Такой роли не существует");
        }

        return user;        
    }
}