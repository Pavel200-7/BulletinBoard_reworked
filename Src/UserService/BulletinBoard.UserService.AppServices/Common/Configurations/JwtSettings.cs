namespace BulletinBoard.NotificationService.AppServices.Common.Configurations;

/// <summary>
/// Настройки JWT.
/// </summary>
public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiresIn { get; set; } = 3600;
}
