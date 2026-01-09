using MediatR;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.ChangeUserName;

public class ChangeUserNameCommand : IRequest<ChangeUserNameCResponse>
{
    public string Id { get; init; }
    public string UserName { get; init; }

    public ChangeUserNameCommand(string id, string userName)
    {
        Id = id;
        UserName = userName;
    }
}
