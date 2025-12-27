using MediatR;


namespace BulletinBoard.UserService.AppServices.User.Commands.DeleteRole;

public class DeleteRoleCommand : IRequest<DeleteRoleCResponse>
{
    public string UserId { get; init; }
    public string Role { get; init; }
}
