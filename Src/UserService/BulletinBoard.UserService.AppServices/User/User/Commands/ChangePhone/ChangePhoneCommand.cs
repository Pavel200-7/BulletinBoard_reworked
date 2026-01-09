using MediatR;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.ChangePhone;

public class ChangePhoneCommand : IRequest<ChangePhoneCResponse>
{
    public string Id { get; init; }
    public string Phone { get; init; }

    public ChangePhoneCommand(string id, string phone)
    {
        Id = id;
        Phone = phone;
    }
}
