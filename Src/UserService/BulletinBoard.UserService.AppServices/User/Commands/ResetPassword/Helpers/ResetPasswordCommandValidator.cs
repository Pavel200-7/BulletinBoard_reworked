using BulletinBoard.UserService.AppServices.User.Commands.ResetPassword;
using FluentValidation;

namespace BulletinBoard.UserService.AppServices.User.Commands.ResetPassword.Helpers;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(c => c.Password)
            .NotNull()
            .NotEmpty()
            .Equal(c => c.ConfirmPassword);
    }
}
