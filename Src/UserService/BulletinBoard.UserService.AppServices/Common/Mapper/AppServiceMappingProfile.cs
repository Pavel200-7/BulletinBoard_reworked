using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.AppServices.User.Commands.Register;
using BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers;
using Microsoft.AspNetCore.Identity;


namespace BulletinBoard.NotificationService.AppServices.Common.Mapper;

public class AppServiceMappingProfile : Profile
{
    public AppServiceMappingProfile()
    {
        CreateMap<RegisterCommand, IdentityUser>();

        CreateMap<IdentityUser, UserAddedEvent>();

        CreateMap<PartialUser, IdentityUser>();
    }
}
