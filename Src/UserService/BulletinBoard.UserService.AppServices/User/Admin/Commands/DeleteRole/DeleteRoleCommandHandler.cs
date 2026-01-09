using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base.FieldFailures;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.User.Admin.Commands.AddRole;
using BulletinBoard.UserService.AppServices.User.Admin.Commands.Helpers.RoleCommandHandler;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Admin.Commands.DeleteRole;

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
        result.Succeeded
            .ThrowBusinessRuleIfFalse(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        _logger.LogInformation("Роль {0} убрана у пользователя с  id {1}.", request.Role, request.UserId);
        return new DeleteRoleCResponse();
    }
}
