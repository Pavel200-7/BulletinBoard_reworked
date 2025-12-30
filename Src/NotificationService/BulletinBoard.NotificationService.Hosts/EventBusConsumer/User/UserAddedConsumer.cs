using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.AppServices.User.Commands.AddUser;
using MassTransit;
using MediatR;

namespace BulletinBoard.NotificationService.Hosts.EventBusConsumer.User;

public class UserAddedConsumer : IConsumer<UserAddedEvent>
{
    private readonly ILogger<UserAddedConsumer> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserAddedConsumer(ILogger<UserAddedConsumer> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<UserAddedEvent> context)
    {
        _logger.LogInformation("Обработка добавления пользователя.");
        var command = _mapper.Map<AddUserCommand>(context.Message);
        await _mediator.Send(command);
    }
}
