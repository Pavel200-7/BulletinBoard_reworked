using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.AppServices.Common.Exceptions;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BulletinBoard.NotificationService.AppServices.User.Commands.SendConfirmationMail;

public class SendConfirmationMailCommandHandler : IRequestHandler<SendConfirmationMailCommand, SendConfirmationMailCResponse>
{
    private readonly ILogger<SendConfirmationMailCommandHandler> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public SendConfirmationMailCommandHandler(
        ILogger<SendConfirmationMailCommandHandler> logger, 
        UserManager<IdentityUser> userManager,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<SendConfirmationMailCResponse> Handle(SendConfirmationMailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        if (user is null)
        {
            throw new NotFoundException("Пользователь с таким id не существует");
        }

        string confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string encodedToken = Base64UrlEncoder.Encode(confirmToken);
        var confirmationStartedEvent = new UserEmailConfirmationStartedEvent(user.Id, encodedToken);
        await _publishEndpoint.Publish(confirmationStartedEvent, cancellationToken);

        return new SendConfirmationMailCResponse();
    }
}
