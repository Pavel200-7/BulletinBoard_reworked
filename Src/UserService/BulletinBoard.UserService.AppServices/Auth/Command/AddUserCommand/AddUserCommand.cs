using MediatR;


namespace BulletinBoard.UserService.AppServices.Auth.Command.AddUserCommand;

public class AddUserCommand : IRequest<AddUserResponse>
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Электронная почта пользователя.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Подтверждение пароля.
    /// </summary>
    public string ConfirmPassword { get; set; }
}
