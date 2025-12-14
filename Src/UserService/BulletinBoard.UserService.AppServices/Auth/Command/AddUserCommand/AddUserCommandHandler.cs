using AutoMapper;
using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter;
using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter.DTO;
using BulletinBoard.UserService.AppServices.Common.Behaviors.TransactionBehavior;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.UserService.AppServices.Auth.Command.AddUserCommand;

[Transaction]
public class AddUserCommandHandler : IRequestHandler<AddUserCommand, AddUserResponse>
{
    private ILogger<AddUserCommandHandler> _logger;
    private IMapper _mapper;
    private IValidator<AddUserCommand> _validator;
    private IAuthServiceAdapter _authServiceAdapter;

    
    public AddUserCommandHandler(
        ILogger<AddUserCommandHandler> logger,
        IMapper mapper, 
        IValidator<AddUserCommand> validator, 
        IAuthServiceAdapter authServiceAdapter)
    {
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
        _authServiceAdapter = authServiceAdapter;
    }

    public async Task<AddUserResponse> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        if (await IsLoginAvailable(request.UserName, cancellationToken) == false)
        {
            throw new BusinessRuleException(nameof(request.UserName), "Данное имя пользователя уже занято");
        }
        if (await IsEmailAvailable(request.Email, cancellationToken) == false)
        {
            throw new BusinessRuleException(nameof(request.Email), "Данная почта уже занята");
        }

        UserCreateDto userDto = _mapper.Map<UserCreateDto>(request);
        bool succed = await _authServiceAdapter.RegisterAsync(userDto, cancellationToken);
        return new AddUserResponse() { IsSucceed = succed };
    }

    private async Task<bool> IsLoginAvailable(string userName, CancellationToken cancellationToken)
    {
        return await _authServiceAdapter.GetByLoginAsync(userName, cancellationToken) is null;
    }

    private async Task<bool> IsEmailAvailable(string email, CancellationToken cancellationToken)
    {
        return await _authServiceAdapter.GetByEmailAsync(email, cancellationToken) is null;
    }
}
