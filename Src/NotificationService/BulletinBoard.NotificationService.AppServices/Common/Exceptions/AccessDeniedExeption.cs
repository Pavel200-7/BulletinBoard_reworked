namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions;

/// <summary>
/// Ошибка ограничения доступа.
/// </summary>
public class AccessDeniedExeption : Exception
{
    public AccessDeniedExeption(string message) 
        : base(message)
    {
    }
}
