using MediatR;


namespace BulletinBoard.UserService.AppServices.User.Commands.ChangePhone;

public class ChangePhoneCommand : IRequest<ChangePhoneCResponse>
{
    public string Id { get; init; }
    public string Phone { get; init; }
}
