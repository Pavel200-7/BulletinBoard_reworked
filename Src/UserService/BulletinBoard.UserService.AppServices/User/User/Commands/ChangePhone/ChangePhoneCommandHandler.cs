using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base.FieldFailures;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using BulletinBoard.UserService.AppServices.User.Helpers.Repository.UserRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.ChangePhone;

public class ChangePhoneCommandHandler : IRequestHandler<ChangePhoneCommand, ChangePhoneCResponse>
{

    private readonly ILogger<ChangePhoneCommandHandler> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _repository;

    public ChangePhoneCommandHandler(
        ILogger<ChangePhoneCommandHandler> logger, 
        UserManager<IdentityUser> userManager,
        IUserRepository repository
        )
    {
        _logger = logger;
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<ChangePhoneCResponse> Handle(ChangePhoneCommand request, CancellationToken cancellationToken)
    {
        await _repository.FindByPhoneAsync(request.Phone, cancellationToken)
            .ThrowBusinessRuleIfNotNull(FieldFailuresConverter.FromSingleFieldError(nameof(request.Phone), "Данный телефон уже занят."));
        var user = await _userManager.FindByIdAsync(request.Id)
            .ThrowNotFoundIfNull("Пользователь с таким id не существует");

        string token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, request.Phone);
        var result = await _userManager.ChangePhoneNumberAsync(user, request.Phone, token);
        result.Succeeded
            .ThrowBusinessRuleIfFalse(FieldFailuresConverter.FromIdentityErrors(result.Errors));

        return new ChangePhoneCResponse();
    }
}
