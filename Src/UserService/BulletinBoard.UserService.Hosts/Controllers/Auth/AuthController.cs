using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulletinBoard.UserService.Hosts.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Защищаем endpoint
public class UserController : ControllerBase
{
    [HttpGet("my-id")]
    public IActionResult GetMyId()
    {
        // Получаем ID из токена (ClaimTypes.NameIdentifier)
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        return Ok(new { UserId = userId, Email = email, Role = role });
    }
}
