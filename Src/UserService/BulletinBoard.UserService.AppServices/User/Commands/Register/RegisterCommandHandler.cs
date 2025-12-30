using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.UserService.AppServices.Common.Behaviors.Transaction;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;
using BulletinBoard.UserService.AppServices.User.Enum;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Commands.Register;

[Transaction]
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCResponse>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public RegisterCommandHandler(
        ILogger<RegisterCommandHandler> logger, 
        IMapper mapper, 
        UserManager<IdentityUser> userManager,
        IUserRepository repository,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<RegisterCResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await ValidateUserUniquenessAsync(request, cancellationToken);
        var user = _mapper.Map<IdentityUser>(request);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        }
        _logger.LogInformation("Пользователь с именем {0} зарегистрирован.", request.UserName);
        await _userManager.AddToRoleAsync(user, Roles.User);
        _logger.LogInformation("Роль {0} добавлена пользователю с именем {1}.", Roles.User, request.UserName);

        var userAddedEvent = _mapper.Map<UserAddedEvent>(user);
        await _publishEndpoint.Publish(userAddedEvent, cancellationToken);

        return new RegisterCResponse();
    }

    private async Task ValidateUserUniquenessAsync(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByNameAsync(request.UserName) is not null)
        {
            throw new BusinessRuleException(nameof(request.UserName), "Данное имя пользователя уже занято.");
        }
        if (await _userManager.FindByEmailAsync(request.Email) is not null)
        {
            throw new BusinessRuleException(nameof(request.Email), "Данный Email уже занят.");
        }
        if (await _repository.FindByPhoneAsync(request.PhoneNumber, cancellationToken) is not null)
        {
            throw new BusinessRuleException(nameof(request.PhoneNumber), "Данный телефон уже занят.");
        }
    }
}
