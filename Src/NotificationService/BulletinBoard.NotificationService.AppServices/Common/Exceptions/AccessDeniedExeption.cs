namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions;

public class AccessDeniedExeption : Exception
{
    public AccessDeniedExeption(string message) 
        : base(message)
    {
    }
}
