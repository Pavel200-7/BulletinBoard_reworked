using BulletinBoard.NotificationService.AppServices.Common.Configurations;
using BulletinBoard.NotificationService.AppServices.Common.IRepository;
using BulletinBoard.NotificationService.AppServices.Notification.Helpers.Mail;
using BulletinBoard.NotificationService.AppServices.Notification.Mail;
using BulletinBoard.NotificationService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace BulletinBoard.NotificationService.AppServices.Notification.User.Commands.SendConfirmMail;

public class SendConfirmMailCommandHandler : IRequestHandler<SendConfirmMailCommand, SendConfirmMailCResponse>
{
    private readonly ILogger<SendConfirmMailCommandHandler> _logger;
    private readonly IEmailSender _emailSender;
    private readonly IQueryRepository<AppUser> _repository;
    private readonly APIGatewayData _gatewayData;

    public SendConfirmMailCommandHandler(
        ILogger<SendConfirmMailCommandHandler> logger, 
        IEmailSender emailSender, 
        IQueryRepository<AppUser> repository,
        IOptions<APIGatewayData> gatewayData)
    {
        _logger = logger;
        _emailSender = emailSender;
        _repository = repository;
        _gatewayData = gatewayData.Value;
    }

    public async Task<SendConfirmMailCResponse> Handle(SendConfirmMailCommand request, CancellationToken cancellationToken)
    {
        string subject = "Подтверждение почты.";
        string messageBody = $"{_gatewayData.Path}/{_gatewayData.ConfirmMailPath}/{request.Id}/{request.Token}";
        var user = await _repository.GetByIdAsync(Guid.Parse(request.Id),
            cancellationToken);
        var mail = new MailData(user!.Email, subject, messageBody);
        await _emailSender.SendEmailAsync(mail);
        return new SendConfirmMailCResponse();
    }
}
