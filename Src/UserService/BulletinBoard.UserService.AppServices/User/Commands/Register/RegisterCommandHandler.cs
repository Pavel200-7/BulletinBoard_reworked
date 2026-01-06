using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Behaviors.Transaction;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Commands.Helpers.RegisterCommandHandler;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.Commands.Register;

[Transaction]
public class RegisterCommandHandler : BaseRegisterCommandHandler,
    IRequestHandler<RegisterCommand, RegisterCResponse>
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
        IPublishEndpoint publishEndpoint) :base(logger, mapper, userManager, publishEndpoint)
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
        await RegisterAsync(user, request.Password, cancellationToken);
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
