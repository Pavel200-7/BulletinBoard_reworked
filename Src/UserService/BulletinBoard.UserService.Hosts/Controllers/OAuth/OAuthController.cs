using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException;
using BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister;
using BulletinBoard.UserService.AppServices.User.User.Queries.GetOAuthProviderURI;
using BulletinBoard.UserService.Hosts.Controllers.Auth;
using BulletinBoard.UserService.Hosts.Controllers.OAuth.Request;
using BulletinBoard.UserService.Hosts.Controllers.OAuth.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace BulletinBoard.UserService.Hosts.Controllers.OAuth;

[ApiController]
[Route("api/v1/[controller]")]
public class OAuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public OAuthController(
        ILogger<AuthController> logger,
        IMapper mapper,
        IMediator mediator)
    {
        _logger = logger;
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpGet("login/{provider}")]
    public async Task<IActionResult> Login(
        [FromRoute] string provider,
        [FromQuery] string returnUrl = "/")
    {
        var state = Guid.NewGuid().ToString("N");
        HttpContext.Session.SetString("oauth_state", state);
        HttpContext.Session.SetString("return_url", returnUrl);

        GetOAuthProviderURIQuery query = new GetOAuthProviderURIQuery(provider, state);
        GetOAuthProviderURIQResponse qResponse = await _mediator.Send(query);
        string authUrl = qResponse.ProviderURI;
        return Redirect(authUrl);
    }

    [HttpGet("login_callback/{provider}")]
    public async Task<IActionResult> LoginCallback(
        [FromRoute] string provider,
        [FromQuery] OAuthLogInCallbackRequest request)
    {
        if (!string.IsNullOrEmpty(request.Error))
        {
            _logger.LogWarning("Возникла ошибка при входе через провайдер OAuth. Ошибка: {0}, описание: {1}",
                request.Error,
                request.ErrorDescription);
            throw new AccessDeniedException(request.Error);
        }

        if (string.IsNullOrEmpty(request.Code))
        {
            throw new AccessDeniedException("Отсутствует код провайдера OAuth.");
        }

        string? expectedState = HttpContext.Session.GetString("oauth_state");
        if (expectedState is null)
        {
            throw new AccessDeniedException("Запрос авторизации устарел.");
        }

        OAuthRegisterCommand command = new OAuthRegisterCommand()
        {
            Provider = provider,
            Code = request.Code,
            State = request.State,
            ExpectedState = expectedState
        };

        OAuthRegisterCResponse cResponse = await _mediator.Send(command);
        OAuthLogInCallbackResponse response = _mapper.Map<OAuthLogInCallbackResponse>(cResponse);
        return Ok(response); 
    }
}
