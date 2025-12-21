using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Behaviors.TransactionBehavior;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common;
using BulletinBoard.UserService.AppServices.User.Enum;
using BulletinBoard.UserService.AppServices.User.Repositiry;
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

    public RegisterCommandHandler(
        ILogger<RegisterCommandHandler> logger, 
        IMapper mapper, 
        UserManager<IdentityUser> userManager,
        IUserRepository repository)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<RegisterCResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await ValidateUserUniquenessAsync(request, cancellationToken);
        var user = _mapper.Map<IdentityUser>(request);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailures.FromIdentityErrors(result.Errors));
        }
        await _userManager.AddToRoleAsync(user, Roles.User);
        return new RegisterCResponse() { IsSucceed = result.Succeeded};
    }

    private async Task ValidateUserUniquenessAsync(RegisterCommand request, CancellationToken cancellationToken)
    {
        var tasks = new[]
        {
            ChechUniqueAsync(() => _userManager.FindByNameAsync(request.UserName),
                nameof(request.UserName),
                "Данное имя пользователя уже занято."),
            ChechUniqueAsync(() => _userManager.FindByEmailAsync(request.Email),
                nameof(request.Email),
                "Данный Email уже занят."),
            ChechUniqueAsync(() => _repository.FindByPhoneAsync(request.PhoneNumber, cancellationToken),
                nameof(request.PhoneNumber),
                "Данный телефон уже занят."),
        };

        await Task.WhenAll(tasks);
    }

    private async Task ChechUniqueAsync<T>(Func<Task<T>> findFunc, string fieldName, string errorMessage) where T : class?
    {
        var user = await findFunc();
        if (user is not null)
        {
            throw new BusinessRuleException(fieldName, errorMessage);
        }
    }
}
