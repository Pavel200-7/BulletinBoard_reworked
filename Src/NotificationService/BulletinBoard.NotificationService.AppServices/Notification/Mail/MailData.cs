namespace BulletinBoard.NotificationService.AppServices.Notification.Mail;

public class MailData
{
    public string ToEmail { get; init; }
    public string Subject { get; init; }
    public string Message { get; init; }

    public MailData(string toEmail, string subject, string message)
    {
        ToEmail = toEmail;
        Subject = subject;
        Message = message;
    }
}
