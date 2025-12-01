using AutoMapper;
using BulletinBoard.UserService.AppServices.Auth.Command.AddUserCommand;
using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter.DTO;


namespace BulletinBoard.UserService.AppServices.Mapper;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<AddUserCommand, UserCreateDto>();
    }
 
}
