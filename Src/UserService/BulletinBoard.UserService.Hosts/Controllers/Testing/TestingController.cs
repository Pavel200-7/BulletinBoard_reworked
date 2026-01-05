using AutoMapper;
using BulletinBoard.NotificationService.AppServices.Common.Exceptions;
using BulletinBoard.NotificationService.Hosts.Controllers.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BulletinBoard.NotificationService.Hosts.Controllers.Testing;

[ApiController]
[Route("api/v1/[controller]")]
public class TestingController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;

    public TestingController(ILogger<AuthController> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("my-id")]
    [Authorize]
    public IActionResult GetMyId()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        return Ok(new { UserId = userId, Email = email, Role = role });
    }

    [HttpGet("is_authorized")]
    [Authorize]
    public IActionResult IsAuthorized()
    {
        return Ok("Да авторизован ты, авторизован!");
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Я живой.");
    }
}
