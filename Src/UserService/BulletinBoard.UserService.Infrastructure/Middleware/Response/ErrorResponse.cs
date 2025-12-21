namespace BulletinBoard.UserService.Infrastructure.Middleware.Response;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string TraceId { get; set; }
}
