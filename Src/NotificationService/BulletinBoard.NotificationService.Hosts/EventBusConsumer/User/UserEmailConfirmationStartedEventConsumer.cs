using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.AppServices.Notification.Commands.SendConfirmMail;
using MassTransit;
using MediatR;

namespace BulletinBoard.NotificationService.Hosts.EventBusConsumer.User;

public class UserEmailConfirmationStartedEventConsumer : IConsumer<UserEmailConfirmationStartedEvent>
{
    private readonly ILogger<UserEmailConfirmationStartedEventConsumer> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserEmailConfirmationStartedEventConsumer(
        ILogger<UserEmailConfirmationStartedEventConsumer> logger, 
        IMediator mediator, 
        IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<UserEmailConfirmationStartedEvent> context)
    {
        _logger.LogInformation("Обработка подтверждения почты.");
        var command = _mapper.Map<SendConfirmMailCommand>(context.Message);
        await _mediator.Send(command);
    }
}
