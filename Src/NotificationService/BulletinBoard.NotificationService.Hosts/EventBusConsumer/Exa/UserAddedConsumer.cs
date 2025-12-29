using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using MassTransit;
using MediatR;

namespace BulletinBoard.NotificationService.Hosts.EventBusConsumer.Exa;

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
        _logger.LogInformation("ID пользователя: {0}, Имя {1}: ", 
            context.Message.Id,
            context.Message.UserName);
    }
}
