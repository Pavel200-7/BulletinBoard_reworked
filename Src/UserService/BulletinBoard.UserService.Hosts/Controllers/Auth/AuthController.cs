using AutoMapper;
using BulletinBoard.UserService.AppServices.User.Commands.ConfirmEmail;
using BulletinBoard.UserService.AppServices.User.Commands.Register;
using BulletinBoard.UserService.AppServices.User.Commands.SendConfirmationMail;
using BulletinBoard.UserService.AppServices.User.Queries.LogIn;
using BulletinBoard.UserService.AppServices.User.Queries.Refresh;
using BulletinBoard.UserService.Hosts.Controllers.Auth.Request;
using BulletinBoard.UserService.Hosts.Controllers.Auth.Response;
using BulletinBoard.UserService.Hosts.Controllers.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BulletinBoard.UserService.Hosts.Controllers.Auth;

[ApiController]
[Route("api/v1/[controller]")]
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

    [HttpPost("/register")]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        RegisterCommand command = _mapper.Map<RegisterCommand>(request);
        RegisterCResponse cResponse = await _mediator.Send(command, cancellationToken);
        RegisterResponse response = _mapper.Map<RegisterResponse>(cResponse);
        return Ok(response);
    }

    [HttpPost("/login")]
    public async Task<IActionResult> LogIn(LogInRequest request, CancellationToken cancellationToken)
    {
        LogInQuery command = _mapper.Map<LogInQuery>(request);
        LogInQResponse qResponse = await _mediator.Send(command, cancellationToken);
        LogInResponse response = _mapper.Map<LogInResponse>(qResponse);
        return Ok(response);
    }

    [HttpPost("/refresh")]
    [Authorize]
    public async Task<IActionResult> Refresh(RefreshRequest request, CancellationToken cancellationToken)
    {
        RefreshQuery query = _mapper.Map<RefreshQuery>(request);
        RefreshQResponse qResponse = await _mediator.Send(query, cancellationToken);
        RefreshResponse response = _mapper.Map<RefreshResponse>(qResponse);
        return Ok(response);
    }

    [HttpGet("/confirm_email/{userId}/{token}")]
    public async Task<IActionResult> ConfirmEmail([FromRoute] string userId, [FromRoute] string token, CancellationToken cancellationToken)
    {
        ConfirmEmailCommand command = new ConfirmEmailCommand(userId, token);
        ConfirmEmailCResponse cResponse = await _mediator.Send(command, cancellationToken);
        ConfirmEmailResponse response = _mapper.Map<ConfirmEmailResponse>(cResponse);
        return Ok(response);
    }

    [HttpGet("/send_confirmation_mail")]
    [Authorize]
    public async Task<IActionResult> ConfirmEmail(CancellationToken cancellationToken)
    {
        string userId = User.GetId().ToString();
        SendConfirmationMailCommand command = new SendConfirmationMailCommand() { Id = userId };
        SendConfirmationMailCResponse cResponse = await _mediator.Send(command, cancellationToken);
        SendConfirmationMailResponse response = _mapper.Map<SendConfirmationMailResponse>(cResponse);
        return Ok(response);
    }
}
