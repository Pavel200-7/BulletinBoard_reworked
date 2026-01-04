using MediatR;

namespace BulletinBoard.NotificationService.AppServices.User.Commands.AddUser;

/// <summary>
/// Добавить пользователя.
/// </summary>
public class AddUserCommand : IRequest<AddUserCResponse>
{
    public string Id { get; init; }
    public string Email { get; init; }
}
