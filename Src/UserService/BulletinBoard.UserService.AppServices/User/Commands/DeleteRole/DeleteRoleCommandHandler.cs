using BulletinBoard.NotificationService.AppServices.Common.Exceptions;
using BulletinBoard.NotificationService.AppServices.Common.Exceptions.Common.FieldFailures;
using BulletinBoard.NotificationService.AppServices.User.Commands.AddRole;
using BulletinBoard.NotificationService.AppServices.User.Commands.Helpers.RoleCommandHandler;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.NotificationService.AppServices.User.Commands.DeleteRole;

public class DeleteRoleCommandHandler : BaseRoleCommandHandler, 
    IRequestHandler<DeleteRoleCommand, DeleteRoleCResponse>
{
    public DeleteRoleCommandHandler(ILogger<AddRoleCommandHandler> logger, UserManager<IdentityUser> userManager)
        : base(logger, userManager)
    {
    }

    public async Task<DeleteRoleCResponse> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await ValidateCommandAndGetUser(request.UserId, request.Role, cancellationToken);
        var result = await _userManager.RemoveFromRoleAsync(user, request.Role);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        }
        _logger.LogInformation("Роль {0} убрана у пользователя с  id {1}.", request.Role, request.UserId);

        return new DeleteRoleCResponse();
    }
}
