using FluentValidation;

namespace BulletinBoard.UserService.AppServices.User.Commands.ChangePhone.Helpers;

public class ChangePhoneCommandValidator : AbstractValidator<ChangePhoneCommand>
{
    public ChangePhoneCommandValidator()
    {
        RuleFor(u => u.Phone)
            .NotNull()
            .NotEmpty()
            .Matches(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$")
            .WithMessage("Некорректный формат телефона.")
            .Length(10, 20)
            .WithMessage("Длина телефона должна быть от 10 до 20 символов.");
    }
}
