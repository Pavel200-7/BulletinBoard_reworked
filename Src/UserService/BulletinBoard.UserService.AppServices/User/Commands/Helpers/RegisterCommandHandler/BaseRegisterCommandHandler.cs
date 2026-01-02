using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;
using BulletinBoard.UserService.AppServices.User.Enum;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Commands.Helpers.RegisterCommandHandler;

public class BaseRegisterCommandHandler
{
    private readonly ILogger<BaseRegisterCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;


    public BaseRegisterCommandHandler(
        ILogger<BaseRegisterCommandHandler> logger,
        IMapper mapper,
        UserManager<IdentityUser> userManager,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }
    public async Task RegisterAsync(IdentityUser user, string password, CancellationToken cancellationToken)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        }

        _logger.LogInformation("Пользователь с именем {0} зарегистрирован.", user.UserName);
        await _userManager.AddToRoleAsync(user, Roles.User);
        _logger.LogInformation("Роль {0} добавлена пользователю с именем {1}.", Roles.User, user.UserName);

        var userAddedEvent = _mapper.Map<UserAddedEvent>(user);
        await _publishEndpoint.Publish(userAddedEvent, cancellationToken);
    }
}
