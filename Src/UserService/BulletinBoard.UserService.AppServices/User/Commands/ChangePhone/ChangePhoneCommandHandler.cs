using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;
using BulletinBoard.UserService.AppServices.User.Commands.AddRole;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.UserService.AppServices.User.Commands.ChangePhone;

public class ChangePhoneCommandHandler : IRequestHandler<ChangePhoneCommand, ChangePhoneCResponse>
{

    private readonly ILogger<ChangePhoneCommandHandler> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _repository;

    public ChangePhoneCommandHandler(
        ILogger<ChangePhoneCommandHandler> logger, 
        UserManager<IdentityUser> userManager,
        IUserRepository repository)
    {
        _logger = logger;
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<ChangePhoneCResponse> Handle(ChangePhoneCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.FindByPhoneAsync(request.Phone, cancellationToken);
        if (user is not null)
        {
            throw new BusinessRuleException(nameof(request.Phone), "Данный телефон уже занят.");
        }

        user = await _userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            throw new NotFoundException("Пользователь с таким id не существует");
        }

        string token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, request.Phone);
        var result = await _userManager.ChangePhoneNumberAsync(user, request.Phone, token);
        if (!result.Succeeded)
        {
            throw new BusinessRuleException(FieldFailuresConverter.FromIdentityErrors(result.Errors));
        }

        return new ChangePhoneCResponse();
    }
}
