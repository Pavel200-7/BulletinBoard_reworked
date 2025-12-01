using AutoMapper;
using BulletinBoard.UserService.AppServices.Auth.Command.AddUserCommand;
using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter;
using BulletinBoard.UserService.Hosts.Controllers.AuthController.Requests;
using BulletinBoard.UserService.Hosts.Controllers.AuthController.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoard.UserService.Hosts.Controllers.AuthController;

[ApiController]
[Route("api/[controller]")]
//[ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status500InternalServerError)]
public class AuthController : ControllerBase
{
    private ILogger<AuthController> _logger;
    private IMapper _mapper;
    private IMediator _mediator;
    public AuthController(ILogger<AuthController> logger, IMapper mapper, IMediator mediator) 
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    /// Зарегистрировать пользователя.
    /// </summary
    /// <remarks>
    /// Пример запроса:
    ///     TODO: сделать пример запроса
    /// </remarks>
    /// <param name="request">Данные пользователя</param>
    /// <returns>Ответ создания</returns>
    [HttpPost, Route("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        AddUserCommand command = _mapper.Map<AddUserCommand>(request);
        AddUserResponse innerResponse = await _mediator.Send(command);
        RegisterResponse response = _mapper.Map<RegisterResponse>(innerResponse);
        return Ok(response);
    }
}
