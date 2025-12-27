using System.Reflection;

namespace BulletinBoard.UserService.AppServices.User.Enum;

public static class Roles
{
    public const string User = "User" ;
    public const string Admin = "Admin";

    public static bool IsRole(string role)
    {
        var roleType = typeof(Roles);
        var fields = roleType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        return fields.Any(f => f.IsLiteral && !f.IsInitOnly && f.GetRawConstantValue()!.ToString() == role);
    }
}
