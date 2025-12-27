using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;
using BulletinBoard.UserService.AppServices.User.Commands.Helpers.RoleCommandHandler;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Commands.AddRole;

public class AddRoleCommandHandler : BaseRoleCommandHandler, 
    IRequestHandler<AddRoleCommand, AddRoleCResponse>
{
    public AddRoleCommandHandler(ILogger<AddRoleCommandHandler> logger, UserManager<IdentityUser> userManager)
        : base(logger, userManager)
    {
    }

    public async Task<AddRoleCResponse> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await ValidateCommandAndGetUser(request.UserId, request.Role, cancellationToken);
        var result = await _userManager.AddToRoleAsync(user, request.Role);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        }

        return new AddRoleCResponse() { IsSucceed = result.Succeeded };
    }
}
