namespace BulletinBoard.UserService.AppServices.Common.Exceptions;

/// <summary>
/// Ошибка ограничения доступа.
/// </summary>
public class AccessDeniedException : Exception
{
    public AccessDeniedException(string message) 
        : base(message)
    {
    }
}
