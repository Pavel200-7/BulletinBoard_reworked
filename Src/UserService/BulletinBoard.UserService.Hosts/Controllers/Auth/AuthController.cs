using AutoMapper;
using BulletinBoard.UserService.AppServices.User.Commands.Register;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn;
using BulletinBoard.UserService.Hosts.Controllers.Auth.Request;
using BulletinBoard.UserService.Hosts.Controllers.Auth.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulletinBoard.UserService.Hosts.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public AuthController(ILogger<AuthController> logger, IMapper mapper, IMediator mediator)
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet("/my-id")]
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

    [HttpPost("/register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        RegisterCommand command = _mapper.Map<RegisterCommand>(request);
        RegisterCResponse cResponse = await _mediator.Send(command);
        RegisterResponse response = _mapper.Map<RegisterResponse>(cResponse);
        return Ok(response);
    }

    [HttpPost("/login")]
    public async Task<IActionResult> LogIn(LogInRequest request)
    {
        LogInQuery command = _mapper.Map<LogInQuery>(request);
        LogInQResponse qResponse = await _mediator.Send(command);
        LogInResponse response = _mapper.Map<LogInResponse>(qResponse);
        return Ok(response);
    }

    [HttpGet("/check_authorize")]
    [Authorize]
    public IActionResult CheckAuthorize()
    {
        return Ok("Вы авторизованы");
    }
}
