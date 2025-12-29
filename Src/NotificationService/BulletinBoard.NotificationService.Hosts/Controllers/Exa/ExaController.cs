using AutoMapper;
using BulletinBoard.NotificationService.AppServices.Notification.Mail;
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
    private readonly IEmailSender _sender;

    public ExaController(ILogger<ExaController> logger, IMapper mapper, IMediator mediator, IEmailSender sender)
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
        _sender = sender;
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

    [HttpGet("/send_test")]
    public async Task<IActionResult>SendTest(CancellationToken cancellationToken)
    {
        await _sender.SendEmailAsync(
            "pavel.yakovlev.elb@gmail.com", 
            "Тестовая отправка рассылки.", 
            "Радуйся, я из будущего - у тебя все сработало, надеюсь с первого раза.");
        return Ok("Я типо что-тосделал, но если нужна инфа о результатах - смотри логи. :)");
    }

}
