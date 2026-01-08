using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.AppServices.Notification.Commands.SendResetPasswordMail;
using MassTransit;
using MediatR;


namespace BulletinBoard.NotificationService.Hosts.EventBusConsumer.User;

public class UserResetPasswordStartedEventConsumer : IConsumer<UserResetPasswordStartedEvent>
{
    private readonly ILogger<UserResetPasswordStartedEventConsumer> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserResetPasswordStartedEventConsumer(
        ILogger<UserResetPasswordStartedEventConsumer> logger, 
        IMediator mediator, 
        IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<UserResetPasswordStartedEvent> context)
    {
        _logger.LogInformation("Обработка отправки сообщения сброса пароля.");
        var command = _mapper.Map<SendResetPasswordMailCommand>(context.Message);
        await _mediator.Send(command);
    }
}
