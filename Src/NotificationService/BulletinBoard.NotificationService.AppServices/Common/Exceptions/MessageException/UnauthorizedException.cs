using BulletinBoard.NotificationService.AppServices.Common.Exceptions.MessageException.Base;


namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions.MessageException;

public class UnauthorizedException : MessageExceptionBase
{
    public UnauthorizedException(string message)
    : base(message)
    {
    }
}
