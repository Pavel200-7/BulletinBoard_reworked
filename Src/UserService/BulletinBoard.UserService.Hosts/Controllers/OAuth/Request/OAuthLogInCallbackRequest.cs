namespace BulletinBoard.NotificationService.Hosts.Controllers.OAuth.Request;

/// <summary>
/// Универсальный запрос - редирект для разных провайдеров oAuth 2.
/// </summary>
public class OAuthLogInCallbackRequest
{
    // Обязательные параметры
    public string Code { get; set; }
    public string State { get; set; }

    // Параметры ошибки (стандарт OAuth 2.0)
    public string Error { get; set; } = string.Empty;
    public string ErrorDescription { get; set; } = string.Empty;
    public string ErrorUri { get; set; } = string.Empty;

    // Дополнительные параметры (зависит от провайдера)
    public string Scope { get; set; } = string.Empty;
    public string SessionState { get; set; } = string.Empty; // Microsoft
    public string AuthUser { get; set; } = string.Empty; // Google
    public string Prompt { get; set; } = string.Empty; // Google
    public string GrantedScopes { get; set; } = string.Empty; // Facebook
    public string IdToken { get; set; } = string.Empty; // Apple, Google (OpenID Connect)
    public string User { get; set; } = string.Empty; // Apple (JSON с данными)
}
