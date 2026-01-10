using System.Text;
using System.Text.Json;


namespace BulletinBoard.UserService.AppServices.User.Admin.Queries.GetUsersData.Helpers;

public class UserSearchCursor
{
    public string LastId { get; set; }
    public string LastUserName { get; set; }
    public string LastEmail { get; set; }
    public DateTime LastCreatedAt { get; set; }
    public string SearchQuery { get; set; }
    public string SortBy { get; set; } // "UserName", "Email", "CreatedAt"
    public bool SortDescending { get; set; }

    public string ToToken()
    {
        var json = JsonSerializer.Serialize(this);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }

    public static UserSearchCursor FromToken(string token)
    {
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        return JsonSerializer.Deserialize<UserSearchCursor>(json)!;
    }
}
