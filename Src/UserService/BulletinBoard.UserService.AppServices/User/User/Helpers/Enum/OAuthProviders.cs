using System.Reflection;


namespace BulletinBoard.UserService.AppServices.User.User.Helpers.Enum;

public class OAuthProviders
{
    public const string GitHub = "github";

    public static bool IsProvider(string provider)
    {
        var providerType = typeof(OAuthProviders);
        var fields = providerType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        return fields.Any(f => f.IsLiteral && !f.IsInitOnly && f.GetRawConstantValue()!.ToString() == provider);
    }
}
