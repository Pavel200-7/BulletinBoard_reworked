namespace BulletinBoard.UserService.AppServices.User.Queries.LogIn.Helpers.JWTGenerator;

public class ClaimsData
{
    public string UserId { get; init; }
    public string Email { get; init; }
    public List<string> Roles { get; init; }
}
