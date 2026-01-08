using AutoMapper;
using BulletinBoard.UserService.AppServices.User.Commands.ChangePhone;
using BulletinBoard.UserService.AppServices.User.Commands.ChangeUserName;
using BulletinBoard.UserService.AppServices.User.Commands.ConfirmEmail;
using BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister;
using BulletinBoard.UserService.AppServices.User.Commands.Register;
using BulletinBoard.UserService.AppServices.User.Commands.ResetPassword;
using BulletinBoard.UserService.AppServices.User.Commands.SendConfirmationMail;
using BulletinBoard.UserService.AppServices.User.Commands.SendResetPasswordMail;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn;
using BulletinBoard.UserService.AppServices.User.Queries.Refresh;
using BulletinBoard.UserService.Hosts.Controllers.Auth.Request;
using BulletinBoard.UserService.Hosts.Controllers.Auth.Response;
using BulletinBoard.UserService.Hosts.Controllers.OAuth.Response;
using BulletinBoard.UserService.Hosts.Controllers.User.Request;
using BulletinBoard.UserService.Hosts.Controllers.User.Response;


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

        CreateMap<SendResetPasswordMailCResponse, SendResetPasswordMailResponse>();

        CreateMap<ResetPasswordRequest, ResetPasswordCommand>();
        CreateMap<ResetPasswordCResponse, ResetPasswordResponse>();

        CreateMap<ChangeUserNameReques, ChangeUserNameCommand>();
        CreateMap<ChangeUserNameCResponse, ChangeUserNameResponse>();

        CreateMap<ChangePhoneRequest, ChangePhoneCommand>();
        CreateMap<ChangePhoneCResponse, ChangePhoneResponse>();
    }
}
