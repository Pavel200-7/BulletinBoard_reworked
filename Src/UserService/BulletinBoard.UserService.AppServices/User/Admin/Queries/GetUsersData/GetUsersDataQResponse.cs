using BulletinBoard.UserService.AppServices.User.Admin.Queries.GetUsersData.Helpers;


namespace BulletinBoard.UserService.AppServices.User.Admin.Queries.GetUsersData;

public class GetUsersDataQResponse
{
    public List<UserData> Users { get; init; }
    public string NextCursor { get; init; } // Для следующей страницы
    public string PreviousCursor { get; init; } // Для возврата назад
    public bool HasNext {  get; init; }
    public bool HasPrevious { get; init; }
}
