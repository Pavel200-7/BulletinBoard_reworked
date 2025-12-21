using AutoMapper;
using BulletinBoard.UserService.AppServices.User.Commands.Register;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn;
using BulletinBoard.UserService.Hosts.Controllers.Auth.Request;
using BulletinBoard.UserService.Hosts.Controllers.Auth.Response;

namespace BulletinBoard.UserService.Hosts.Common.Mapper;

public class HostsMappingProfile : Profile
{
    public HostsMappingProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();
        CreateMap<RegisterCResponse, RegisterResponse>();

        CreateMap<LogInRequest, LogInQuery>();
        CreateMap<LogInQResponse, LogInResponse>();
    }
}
