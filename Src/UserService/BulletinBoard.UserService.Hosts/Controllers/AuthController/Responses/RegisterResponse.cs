namespace BulletinBoard.UserService.Hosts.Controllers.AuthController.Responses;

/// <summary>
/// Ответ на запрос создания пользователя.
/// </summary>
public class RegisterResponse
{
    /// <summary>
    /// Был ли создан пользователь.
    /// </summary>
    public bool IsSucceed { get; set; }
}
