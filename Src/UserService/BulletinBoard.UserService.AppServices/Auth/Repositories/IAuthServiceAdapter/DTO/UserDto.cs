namespace BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter.DTO;

/// <summary>
/// Дто пользователя.
/// </summary>
public class UserDto
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
}
