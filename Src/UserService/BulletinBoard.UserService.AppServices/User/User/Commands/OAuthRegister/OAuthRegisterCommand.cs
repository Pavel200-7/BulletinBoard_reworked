using MediatR;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister;

public class OAuthRegisterCommand : IRequest<OAuthRegisterCResponse>
{
    public string Provider { get; init; }
    public string Code { get; init; }
    public string State { get; init; }
    public string ExpectedState { get; init; }
}
