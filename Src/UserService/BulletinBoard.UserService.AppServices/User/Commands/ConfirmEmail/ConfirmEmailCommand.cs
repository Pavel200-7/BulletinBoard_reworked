using MediatR;

namespace BulletinBoard.UserService.AppServices.User.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<ConfirmEmailCResponse>
{
    public string UserId { get; init; }
    public string Token { get; init; }

    public ConfirmEmailCommand(string userId, string token)
    {
        UserId = userId;
        Token = token;
    }
}
