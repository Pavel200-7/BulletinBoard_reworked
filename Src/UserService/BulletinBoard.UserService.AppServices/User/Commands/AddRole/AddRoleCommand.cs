using MediatR;


namespace BulletinBoard.NotificationService.AppServices.User.Commands.AddRole;

/// <summary>
/// Добавить роль.
/// </summary>
public class AddRoleCommand : IRequest<AddRoleCResponse>
{
    public string UserId { get; init; }
    public string Role { get; init; }
}
