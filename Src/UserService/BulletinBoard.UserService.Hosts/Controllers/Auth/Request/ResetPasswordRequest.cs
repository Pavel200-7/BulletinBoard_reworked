namespace BulletinBoard.UserService.Hosts.Controllers.Auth.Request;

public class ResetPasswordRequest
{
    public string Email { get; init; }
    public string Token { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
