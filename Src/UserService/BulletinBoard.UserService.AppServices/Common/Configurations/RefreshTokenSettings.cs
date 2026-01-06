namespace BulletinBoard.UserService.AppServices.Common.Configurations;

/// <summary>
/// Настройки токена обновления.
/// </summary>
public class RefreshTokenSettings
{
    public int ExpiresIn { get; set; } = 669600;
}
