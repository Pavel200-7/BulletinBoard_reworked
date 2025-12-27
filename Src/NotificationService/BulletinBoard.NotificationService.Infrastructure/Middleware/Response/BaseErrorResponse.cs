namespace BulletinBoard.NotificationService.Infrastructure.Middleware.Response;

public class BaseErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string TraceId { get; set; }
}
