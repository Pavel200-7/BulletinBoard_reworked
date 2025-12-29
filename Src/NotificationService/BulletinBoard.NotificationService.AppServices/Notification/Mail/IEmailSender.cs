namespace BulletinBoard.NotificationService.AppServices.Notification.Mail;

/// <summary>
/// Класс отправки почты.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Отправить почту.
    /// </summary>
    /// <param name="toEmail">Адрес электронной почты.</param>
    /// <param name="subject">Тема сообщения.</param>
    /// <param name="message">Сообщение.</param>
    /// <returns></returns>
    public Task SendEmailAsync(string toEmail, string subject, string message);
}
