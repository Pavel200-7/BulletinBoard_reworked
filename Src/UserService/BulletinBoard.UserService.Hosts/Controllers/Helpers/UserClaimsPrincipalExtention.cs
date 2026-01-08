using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException;
using System.Security.Claims;


namespace BulletinBoard.UserService.Hosts.Controllers.Helpers;

/// <summary>
/// Доставатель клеймов JWT.
/// </summary>
public static class UserClaimsPrincipalExtention
{
    /// <summary>
    /// Получить id пользователя.
    /// </summary>
    /// <returns>Id пользователя.</returns>
    /// <exception cref="AccessDeniedException">Ошибка доступа из-за отсутствуия нужного клейма.</exception>
    public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new AccessDeniedException("JWT не содержит id пользователя.");
        }
        return Guid.Parse(userId);
    }

    /// <summary>
    /// Получить email пользователя.
    /// </summary>
    /// <returns>Email пользователя.</returns>
    /// <exception cref="AccessDeniedException">Ошибка доступа из-за отсутствуия нужного клейма.</exception>
    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            throw new AccessDeniedException("JWT не содержит email пользователя.");
        }
        return email;
    }

    /// <summary>
    /// Получить роли пользователя.
    /// </summary>
    /// <returns>Роли пользователя.</returns>
    /// <exception cref="AccessDeniedException">Ошибка доступа из-за отсутствуия нужного клейма.</exception>
    public static List<string> GetRoles(this ClaimsPrincipal claimsPrincipal)
    {
        var userRoles = claimsPrincipal.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
        if (!userRoles.Any())
        {
            throw new AccessDeniedException("JWT не содержит ролей пользователя.");
        }
        return userRoles;
    }
}
