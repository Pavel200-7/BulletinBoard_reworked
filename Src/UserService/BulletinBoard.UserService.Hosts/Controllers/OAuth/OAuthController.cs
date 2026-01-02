//using AutoMapper;
//using BulletinBoard.UserService.Hosts.Controllers.Auth;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;


//namespace BulletinBoard.UserService.Hosts.Controllers.OAuth;

//[ApiController]
//[Route("api/v1/[controller]")]
//public class OAuthController : ControllerBase
//{
//    private readonly ILogger<AuthController> _logger;
//    private readonly IConfiguration _config;
//    private readonly IMapper _mapper;
//    private readonly IMediator _mediator;

//    public OAuthController(
//        ILogger<AuthController> logger, 
//        IConfiguration config,
//        IMapper mapper, 
//        IMediator mediator)
//    {
//        _logger = logger;
//        _config = config;
//        _mapper = mapper;
//        _mediator = mediator;
//    }

//    [HttpPost("/login/github")]
//    public async Task<IActionResult> Login([FromQuery] string returnUrl = "/")
//    {
//        // 1. Генерируем state для защиты от CSRF
//        var state = Guid.NewGuid().ToString("N");

//        // Сохраняем state в сессии (или в кеш/БД)
//        HttpContext.Session.SetString("github_oauth_state", state);
//        HttpContext.Session.SetString("github_return_url", returnUrl);

//        // 2. Берем настройки из конфигурации
//        var clientId = _config["GitHub:ClientId"];
//        var redirectUri = _config["GitHub:RedirectUri"];

//        // 3. Формируем scope (права доступа)
//        var scopes = new List<string>
//        {
//            "read:user",     // Чтение данных профиля
//            "user:email"     // Доступ к email
//        };
//        var scope = string.Join(" ", scopes);

//        // 4. Собираем URL для редиректа на GitHub
//        var githubAuthUrl = "https://github.com/login/oauth/authorize?" +
//            $"client_id={Uri.EscapeDataString(clientId)}&" +
//            $"redirect_uri={Uri.EscapeDataString(redirectUri)}&" +
//            $"scope={Uri.EscapeDataString(scope)}&" +
//            $"state={Uri.EscapeDataString(state)}&" +
//            "allow_signup=false"; // Запрещаем регистрацию на GitHub через наше приложение

//        _logger.LogInformation("Redirecting to GitHub: {Url}", githubAuthUrl);

//        // 5. Перенаправляем пользователя на GitHub
//        return Redirect(githubAuthUrl);
//    }
//}
