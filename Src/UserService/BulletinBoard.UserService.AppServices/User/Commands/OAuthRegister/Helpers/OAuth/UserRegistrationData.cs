namespace BulletinBoard.NotificationService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth;

/// <summary>
/// Информация для регистрации с частичными данными.
/// </summary>
public class UserRegistrationData
{
    public string Email { get; set; }

    public UserRegistrationData(string email)
    {
        Email = email;
    }
}
