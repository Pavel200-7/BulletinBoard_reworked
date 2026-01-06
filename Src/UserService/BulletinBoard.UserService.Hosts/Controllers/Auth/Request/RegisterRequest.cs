namespace BulletinBoard.UserService.Hosts.Controllers.Auth.Request;

public class RegisterRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
