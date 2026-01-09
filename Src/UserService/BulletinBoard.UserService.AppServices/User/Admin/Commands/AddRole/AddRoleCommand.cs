using MediatR;


namespace BulletinBoard.UserService.AppServices.User.Admin.Commands.AddRole;

/// <summary>
/// Добавить роль.
/// </summary>
public class AddRoleCommand : IRequest<AddRoleCResponse>
{
    public string UserId { get; init; }
    public string Role { get; init; }
}
