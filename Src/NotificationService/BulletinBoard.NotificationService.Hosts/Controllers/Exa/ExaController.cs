using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;


namespace BulletinBoard.UserService.Hosts.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class ExaController : ControllerBase
{
    private readonly ILogger<ExaController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ExaController(ILogger<ExaController> logger, IMapper mapper, IMediator mediator)
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet("/my-id")]
    [Authorize]
    public IActionResult GetMyId()
    {
        var userId = User.FindFirst(ClaimTypes.Sid)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(JsonSerializer.Serialize(User.Claims));

        return Ok(new { UserId = userId, Email = email, Role = role });
    }

    [HttpGet("/check_authorize")]
    [Authorize]
    public IActionResult CheckAuthorize()
    {
        return Ok("Вы авторизованы");
    }
}
