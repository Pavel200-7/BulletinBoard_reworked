using AutoMapper;
using BulletinBoard.UserService.AppServices.User.Commands.Register;
using Microsoft.AspNetCore.Identity;


namespace BulletinBoard.UserService.AppServices.Common.Mapper;

public class AppServiceMappingProfile : Profile
{
    public AppServiceMappingProfile()
    {
        CreateMap<RegisterCommand, IdentityUser>();
    }
}
