using BulletinBoard.UserService.AppServices.User.User.Commands.ChangeUserName;
using FluentValidation;

namespace BulletinBoard.UserService.AppServices.User.User.Commands.ChangeUserName.Helpers;

public class ChangeUserNameCommandValidator : AbstractValidator<ChangeUserNameCommand>
{
    public ChangeUserNameCommandValidator()
    {
        RuleFor(u => u.UserName)
            .NotNull()
            .NotEmpty()
            .Length(8, 50)
            .Matches(@"^[а-яА-ЯёЁa-zA-Z][а-яА-ЯёЁa-zA-Z0-9_]{7,49}$")
            .WithMessage("Имя пользователя должно начинаться с буквы и содержать только русские/английские буквы, цифры и подчеркивание.");
    }
}
