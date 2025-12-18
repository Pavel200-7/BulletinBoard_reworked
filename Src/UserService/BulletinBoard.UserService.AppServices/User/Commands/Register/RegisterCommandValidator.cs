using FluentValidation;
using System.Text.RegularExpressions;


namespace BulletinBoard.UserService.AppServices.User.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(u => u.UserName)
            .NotNull()
            .NotEmpty()
            .Length(8, 50)
            .Matches(@"^[а-яА-ЯёЁa-zA-Z][а-яА-ЯёЁa-zA-Z0-9_]{7,49}$")
            .WithMessage("Имя пользователя должно начинаться с буквы и содержать только русские/английские буквы, цифры и подчеркивание.");

        RuleFor(u => u.Email)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Пожалуйста, укажите корректный email адрес.");

        RuleFor(u => u.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .Matches(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$")
            .WithMessage("Некорректный формат телефона.")
            .Length(10, 20)
            .WithMessage("Длина телефона должна быть от 10 до 20 символов.");

        RuleFor(u => u.Password)
            .NotNull()
            .NotEmpty()
            .Equal(u => u.ConfirmPassword);
    }
}
