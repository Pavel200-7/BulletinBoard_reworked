using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister.Helpers;
using BulletinBoard.UserService.AppServices.User.User.Commands.Register;
using Microsoft.AspNetCore.Identity;


namespace BulletinBoard.UserService.AppServices.Common.Mapper;

public class AppServiceMappingProfile : Profile
{
    public AppServiceMappingProfile()
    {
        CreateMap<RegisterCommand, IdentityUser>();

        CreateMap<IdentityUser, UserAddedEvent>();

        CreateMap<PartialUser, IdentityUser>();
    }
}
