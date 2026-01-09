namespace BulletinBoard.NotificationService.AppServices.Notification.Helpers.Mail;

/// <summary>
/// Класс отправки почты.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Отправить почту.
    /// </summary>
    public Task SendEmailAsync(MailData mail);
    /// <summary>
    /// Отправить почту.
    /// </summary>
    public Task SendEmailsAsync(List<MailData> mails);
}
