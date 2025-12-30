using BulletinBoard.UserService.AppServices.Common.Exceptions;
using System.Security.Claims;


namespace BulletinBoard.UserService.Hosts.Controllers.Helpers;

public static class UserClaimsPrincipalExtention
{
    public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new AccessDeniedExeption("JWT не содержит id пользователя.");
        }
        return Guid.Parse(userId);
    }

    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;
        if (email == null)
        {
            throw new AccessDeniedExeption("JWT не содержит email пользователя.");
        }
        return email;
    }

    public static List<string> GetRoles(this ClaimsPrincipal claimsPrincipal)
    {
        var userRoles = claimsPrincipal.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
        if (!userRoles.Any())
        {
            throw new AccessDeniedExeption("JWT не содержит ролей пользователя.");
        }
        return userRoles;
    }
}
