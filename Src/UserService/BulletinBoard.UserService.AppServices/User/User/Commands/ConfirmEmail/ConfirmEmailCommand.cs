using MediatR;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.ConfirmEmail;

/// <summary>
/// Подтвердить почту.
/// </summary>
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
