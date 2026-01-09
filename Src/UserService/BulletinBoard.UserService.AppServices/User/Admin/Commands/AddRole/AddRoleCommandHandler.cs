using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException;
using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base.FieldFailures;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.User.Admin.Commands.Helpers.RoleCommandHandler;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Admin.Commands.AddRole;

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
        result.Succeeded
            .ThrowBusinessRuleIfFalse(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        _logger.LogInformation("Роль {0} добавлена пользователю с  id {1}.", request.Role, request.UserId);
        return new AddRoleCResponse();
    }
}
