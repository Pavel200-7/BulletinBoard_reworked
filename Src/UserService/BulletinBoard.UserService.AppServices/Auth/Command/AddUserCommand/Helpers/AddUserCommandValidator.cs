using FluentValidation;


namespace BulletinBoard.UserService.AppServices.Auth.Command.AddUserCommand.Helpers;

public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleFor(c => c.UserName)
            .Equal("111")
            .WithMessage("Пиздато.");
    }
}
