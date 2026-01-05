using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.AppServices.User.Commands.ConfirmEmail;
using BulletinBoard.NotificationService.AppServices.User.Commands.Register;
using BulletinBoard.NotificationService.AppServices.User.Commands.SendConfirmationMail;
using BulletinBoard.NotificationService.AppServices.User.Queries.LogIn;
using BulletinBoard.NotificationService.AppServices.User.Queries.Refresh;
using BulletinBoard.NotificationService.Hosts.Controllers.Auth.Request;
using BulletinBoard.NotificationService.Hosts.Controllers.Auth.Response;
using BulletinBoard.NotificationService.Hosts.Controllers.OAuth.Response;
using BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister;
using Microsoft.AspNetCore.Identity;


namespace BulletinBoard.NotificationService.Hosts.Common.Mapper;

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
