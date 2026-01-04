namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message)
    : base(message)
    {
    }
}
