using AutoMapper;
using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter.DTO;
using BulletinBoard.UserService.Infrastructure.Identity.Entities;


namespace BulletinBoard.Infrastructure.DataAccess.Mapper;

public class InfrastructureMappingProfile : Profile
{
    public InfrastructureMappingProfile() 
    {
        CreateMap<UserCreateDto, ApplicationUser>();
        CreateMap<ApplicationUser, UserDto>()
            .ReverseMap();
    }
}
