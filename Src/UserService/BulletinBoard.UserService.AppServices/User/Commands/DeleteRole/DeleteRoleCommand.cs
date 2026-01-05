using MediatR;


namespace BulletinBoard.NotificationService.AppServices.User.Commands.DeleteRole;

/// <summary>
/// Удалить роль.
/// </summary>
public class DeleteRoleCommand : IRequest<DeleteRoleCResponse>
{
    public string UserId { get; init; }
    public string Role { get; init; }
}
