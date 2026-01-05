using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.NotificationService.Hosts.Controllers.Home;

[ApiController]
[Route("api/v1/[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Home()
    {
        return Ok("Вы на главной странице, да тут пока начего нет.");
    }
}
