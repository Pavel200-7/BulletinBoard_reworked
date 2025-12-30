using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.AppServices.Notification.Commands.SendConfirmMail;
using BulletinBoard.NotificationService.AppServices.User.Commands.AddUser;


namespace BulletinBoard.UserService.Hosts.Common.Mapper;

public class HostsMappingProfile : Profile
{
    public HostsMappingProfile()
    {
        CreateMap<UserAddedEvent, AddUserCommand>();
        CreateMap<UserEmailConfirmationStartedEvent, SendConfirmMailCommand>();

    }
}
