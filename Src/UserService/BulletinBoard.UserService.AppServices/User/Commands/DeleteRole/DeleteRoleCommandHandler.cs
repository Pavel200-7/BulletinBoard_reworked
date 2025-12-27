using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;
using BulletinBoard.UserService.AppServices.User.Commands.AddRole;
using BulletinBoard.UserService.AppServices.User.Commands.Helpers.RoleCommandHandler;
using BulletinBoard.UserService.AppServices.User.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.UserService.AppServices.User.Commands.DeleteRole;

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

        return new DeleteRoleCResponse() { IsSucceed = result.Succeeded };
    }
}
