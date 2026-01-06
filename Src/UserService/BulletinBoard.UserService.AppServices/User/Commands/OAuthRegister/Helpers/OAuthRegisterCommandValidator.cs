using BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister;
using FluentValidation;


namespace BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers;

public class OAuthRegisterCommandValidator : AbstractValidator<OAuthRegisterCommand>
{
    public OAuthRegisterCommandValidator()
    {
        RuleFor(command => command.State)
            .Equal(c => c.ExpectedState)
            .WithMessage("State переданный при начале запроса к провайдеру oAuth 2 отличается от текущего.");
    }
}
