using AutoMapper;
using BulletinBoard.NotificationService.AppServices.Common.IRepository;
using BulletinBoard.NotificationService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;


namespace BulletinBoard.NotificationService.AppServices.User.Commands.AddUser;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, AddUserCResponse>
{
    private readonly ILogger<AddUserCommandHandler> _logger;
    private readonly ICommandRepository<AppUser> _repository;

    public AddUserCommandHandler(ILogger<AddUserCommandHandler> logger, ICommandRepository<AppUser> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<AddUserCResponse> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        AppUser user = new AppUser(request.Id, request.Email);
        await _repository.AddAsync(user, cancellationToken);
        _logger.LogInformation("Пользователь с именем {0} добавлен.", request.Id);
        return new AddUserCResponse();
    }
}
