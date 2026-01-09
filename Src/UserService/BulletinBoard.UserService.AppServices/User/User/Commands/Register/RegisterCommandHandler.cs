using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Behaviors.Transaction;
using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base.FieldFailures;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.User.Helpers.Repositiry;
using BulletinBoard.UserService.AppServices.User.User.Commands.Helpers.RegisterCommandHandler;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.Register;

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
        await _userManager.FindByNameAsync(request.UserName)
            .ThrowBusinessRuleIfNotNull(FieldFailuresConverter.FromSingleFieldError(nameof(request.UserName), "Данное имя пользователя уже занято."));
        await _userManager.FindByEmailAsync(request.Email)
            .ThrowBusinessRuleIfNotNull(FieldFailuresConverter.FromSingleFieldError(nameof(request.Email), "Данный Email уже занят."));
        await _repository.FindByPhoneAsync(request.PhoneNumber, cancellationToken)
            .ThrowBusinessRuleIfNotNull(FieldFailuresConverter.FromSingleFieldError(nameof(request.PhoneNumber), "Данный телефон уже занят."));
    }
}
