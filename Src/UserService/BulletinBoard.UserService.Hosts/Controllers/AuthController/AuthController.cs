using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter;
using BulletinBoard.UserService.Hosts.Controllers.AuthController.Requests;
using BulletinBoard.UserService.Hosts.Controllers.AuthController.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.UserService.Hosts.Controllers.AuthController;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private ILogger<AuthController> _logger;
    private IAuthServiceAdapter _authService;
    public AuthController(ILogger<AuthController> logger, IAuthServiceAdapter authService) 
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpPost, Route("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        //_authService.
        return Ok("111");
    }
}
