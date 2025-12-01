using AutoMapper;
using BulletinBoard.UserService.AppServices.Auth.Command.AddUserCommand;
using BulletinBoard.UserService.Hosts.Controllers.AuthController.Requests;
using BulletinBoard.UserService.Hosts.Controllers.AuthController.Responses;

namespace BulletinBoard.UserService.Hosts.Mapper;

public class HostMappingProfile : Profile
{
    public HostMappingProfile() 
    {
        CreateMap<RegisterRequest, AddUserCommand>();
        CreateMap<AddUserResponse, RegisterResponse>();
    }
}
