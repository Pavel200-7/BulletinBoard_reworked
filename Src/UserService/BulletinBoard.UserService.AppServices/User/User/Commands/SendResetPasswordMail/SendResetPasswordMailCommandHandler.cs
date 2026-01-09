using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


namespace BulletinBoard.UserService.AppServices.User.User.Commands.SendResetPasswordMail;

public class SendResetPasswordMailCommandHandler : IRequestHandler<SendResetPasswordMailCommand, SendResetPasswordMailCResponse>
{
    private readonly ILogger<SendResetPasswordMailCommandHandler> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public SendResetPasswordMailCommandHandler(
        ILogger<SendResetPasswordMailCommandHandler> logger, 
        UserManager<IdentityUser> userManager, 
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<SendResetPasswordMailCResponse> Handle(SendResetPasswordMailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            .ThrowNotFoundIfNull("Пользователь с таким email не существует");

        string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        string encodedToken = Base64UrlEncoder.Encode(passwordResetToken);
        var passwordChangeStartedEvent = new UserResetPasswordStartedEvent(user.Id, encodedToken);
        await _publishEndpoint.Publish(passwordChangeStartedEvent, cancellationToken);

        return new SendResetPasswordMailCResponse();
    }
}
