using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.UserService.AppServices.User.Commands.ConfirmEmail;
using BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister;
using BulletinBoard.UserService.AppServices.User.Commands.Register;
using BulletinBoard.UserService.AppServices.User.Commands.SendConfirmationMail;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn;
using BulletinBoard.UserService.AppServices.User.Queries.Refresh;
using BulletinBoard.UserService.Hosts.Controllers.Auth.Request;
using BulletinBoard.UserService.Hosts.Controllers.Auth.Response;
using BulletinBoard.UserService.Hosts.Controllers.OAuth.Response;
using Microsoft.AspNetCore.Identity;


namespace BulletinBoard.UserService.Hosts.Common.Mapper;

public class HostsMappingProfile : Profile
{
    public HostsMappingProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();
        CreateMap<RegisterCResponse, RegisterResponse>();

        CreateMap<LogInRequest, LogInQuery>();
        CreateMap<LogInQResponse, LogInResponse>();

        CreateMap<ConfirmEmailCResponse, ConfirmEmailResponse>();

        CreateMap<RefreshRequest, RefreshQuery>();
        CreateMap<RefreshQResponse, RefreshResponse>();

        CreateMap<SendConfirmationMailCResponse, SendConfirmationMailResponse>();

        CreateMap<OAuthRegisterCResponse, OAuthLogInCallbackResponse>();


    }
}
