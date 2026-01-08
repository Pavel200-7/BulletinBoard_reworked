using AutoMapper;
using BulletinBoard.UserService.AppServices.User.Commands.ChangePhone;
using BulletinBoard.UserService.AppServices.User.Commands.ChangeUserName;
using BulletinBoard.UserService.Hosts.Controllers.Auth;
using BulletinBoard.UserService.Hosts.Controllers.Helpers;
using BulletinBoard.UserService.Hosts.Controllers.User.Request;
using BulletinBoard.UserService.Hosts.Controllers.User.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BulletinBoard.UserService.Hosts.Controllers.User;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public UserController(ILogger<AuthController> logger, IMapper mapper, IMediator mediator)
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("change_name")]
    [Authorize]
    public async Task<IActionResult> ChangeName(ChangeUserNameReques request, CancellationToken cancellationToken)
    {
        ChangeUserNameCommand command = new ChangeUserNameCommand(
            request.UserName,
            User.GetId().ToString());
        ChangeUserNameCResponse cResponse = await _mediator.Send(command, cancellationToken);
        ChangeUserNameResponse response = _mapper.Map<ChangeUserNameResponse>(cResponse);
        return Ok(response);
    }

    [HttpPost("change_phone")]
    [Authorize]
    public async Task<IActionResult> ChangePhome(ChangePhoneRequest request, CancellationToken cancellationToken)
    {
        ChangePhoneCommand command = new ChangePhoneCommand(
            request.Phone, 
            User.GetId().ToString());
        ChangePhoneCResponse cResponse = await _mediator.Send(command, cancellationToken);
        ChangePhoneResponse response = _mapper.Map<ChangePhoneResponse>(cResponse);
        return Ok(response);
    }
}
