using BulletinBoard.NotificationService.AppServices.Notification.Mail;
using BulletinBoard.NotificationService.Infrastructure.Common.Configurations;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BulletinBoard.Infrastructure.DataAccess.Contexts.User.EmailSender;

public class YandexSmtpEmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    private readonly ILogger<YandexSmtpEmailSender> _logger;

    public YandexSmtpEmailSender(
        IOptions<EmailSettings> settings,
        ILogger<YandexSmtpEmailSender> logger)
    {
        _settings = settings.Value;
        _logger = logger;
        _logger.LogInformation($"Settings: {_settings.Username}, Server: {_settings.SmtpServer}:{_settings.Port}");
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        try
        {
            _logger.LogInformation($"Sending email to: {toEmail}");

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_settings.FromName, _settings.Username));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
            _logger.LogInformation("Connected to SMTP server");
            await client.AuthenticateAsync(_settings.Username, _settings.Password);
            _logger.LogInformation("Authenticated successfully");

            string answer = await client.SendAsync(emailMessage);
            _logger.LogInformation($"answer {answer}");
            _logger.LogInformation("Email sent to server");

            await client.DisconnectAsync(true);

            _logger.LogInformation($"Email sent successfully to: {toEmail}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send email to: {toEmail}");
            throw new Exception($"Failed to send email: {ex.Message}", ex);
        }
    }
}