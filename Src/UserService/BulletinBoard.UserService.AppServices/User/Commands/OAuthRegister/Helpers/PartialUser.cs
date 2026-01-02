namespace BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers;

/// <summary>
/// Частичные данные пользователя для регистрации через oauth 2.
/// Здесь в качестве имени пользователя выступает его email,
/// телефон не задается, а пароль генерируется случайно 
/// (пароль по идее временный и пользователь должен его сменить сам).
/// </summary>
public class PartialUser
{
    public string UserName { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
    public string Password { get; init; }

    public PartialUser(string email)
    {
        UserName = email;
        Email = email;
        PhoneNumber = "";
        Password = Guid.NewGuid().ToString();
    }
}
