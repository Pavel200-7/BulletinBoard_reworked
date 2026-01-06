using MediatR;


namespace BulletinBoard.UserService.AppServices.User.Commands.ChangeUserName;

public class ChangeUserNameCommand : IRequest<ChangeUserNameCResponse>
{
    public string Id { get; init; }
    public string UserName { get; init; }
}
