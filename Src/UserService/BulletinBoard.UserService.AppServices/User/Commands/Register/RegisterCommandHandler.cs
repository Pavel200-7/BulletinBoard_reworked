using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common;
using BulletinBoard.UserService.AppServices.User.Enum;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.UserService.AppServices.User.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private ILogger<RegisterCommandHandler> _logger;
    private IMapper _mapper;
    private UserManager<IdentityUser> _userManager;
    private IUserRepository _repository;

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

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        await ValidateUserUniquenessAsync(request, cancellationToken);
        var user = _mapper.Map<IdentityUser>(request);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailures.FromIdentityErrors(result.Errors));
        }
        await _userManager.AddToRoleAsync(user, Roles.User);
        return new RegisterResponse();
    }

    private async Task ValidateUserUniquenessAsync(RegisterCommand request, CancellationToken cancellationToken)
    {
        var tasks = new[]
        {
            ChechUniqueAsync(() => _userManager.FindByNameAsync(request.UserName),
                nameof(request.UserName),
                "User name must be unique."),
            ChechUniqueAsync(() => _userManager.FindByEmailAsync(request.Email),
                nameof(request.Email),
                "Email must be unique."),
            ChechUniqueAsync(() => _repository.FindByPhoneAsync(request.PhoneNumber, cancellationToken),
                nameof(request.PhoneNumber),
                "Phone number must be unique."),
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
