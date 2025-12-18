using MediatR;


namespace BulletinBoard.UserService.AppServices.User.Commands.Register;

public class RegisterCommand : IRequest<RegisterResponse>
{
    public string UserName { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public string Password { get; init; }
    public string ConfirmPassword { get; init; }
}
