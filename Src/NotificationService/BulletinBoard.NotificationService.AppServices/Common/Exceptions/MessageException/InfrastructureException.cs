using BulletinBoard.NotificationService.AppServices.Common.Exceptions.MessageException.Base;


namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions.MessageException;

public class InfrastructureException : MessageExceptionBase
{
    public InfrastructureException(string message)
        : base(message)
    {
    }
}
