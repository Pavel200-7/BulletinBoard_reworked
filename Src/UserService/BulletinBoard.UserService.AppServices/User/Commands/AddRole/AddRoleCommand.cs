using MediatR;


namespace BulletinBoard.UserService.AppServices.User.Commands.AddRole;

public class AddRoleCommand : IRequest<AddRoleCResponse>
{
    public string UserId { get; init; }
    public string Role { get; init; }
}
