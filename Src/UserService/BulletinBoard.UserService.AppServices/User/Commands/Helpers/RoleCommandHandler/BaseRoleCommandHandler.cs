using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base.FieldFailures;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using BulletinBoard.UserService.AppServices.User.Commands.AddRole;
using BulletinBoard.UserService.AppServices.User.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Commands.Helpers.RoleCommandHandler;

/// <summary>
/// Базовый обработчик изменения ролей.
/// </summary>
public abstract class BaseRoleCommandHandler
{
    protected readonly ILogger<AddRoleCommandHandler> _logger;
    protected readonly UserManager<IdentityUser> _userManager;

    protected BaseRoleCommandHandler(ILogger<AddRoleCommandHandler> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    /// <summary>
    /// Валидатор команды изменения ролей.
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="role">Роль</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Пользователь</returns>
    /// <exception cref="NotFoundException">Пользователь не найден</exception>
    /// <exception cref="BusinessRuleException">Такой роли не существует</exception>
    public async Task<IdentityUser> ValidateCommandAndGetUser(string userId, string role, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId)
            .ThrowNotFoundIfNull("Пользователь с таким id не существует");
        Roles.IsRole(role)
            .ThrowBusinessRuleIfFalse(FieldFailuresConverter.FromSingleFieldError("Role", "Такой роли не существует"));

        return user;        
    }
}