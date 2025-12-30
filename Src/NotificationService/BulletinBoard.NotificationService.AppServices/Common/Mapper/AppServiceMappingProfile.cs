using AutoMapper;
using BulletinBoard.NotificationService.AppServices.User.Commands.AddUser;
using BulletinBoard.NotificationService.Domain.Entities;


namespace BulletinBoard.NotificationService.AppServices.Common.Mapper;

public class AppServiceMappingProfile : Profile
{
    public AppServiceMappingProfile()
    {
        CreateMap<AddUserCommandHandler, AppUser>();
    }
}
