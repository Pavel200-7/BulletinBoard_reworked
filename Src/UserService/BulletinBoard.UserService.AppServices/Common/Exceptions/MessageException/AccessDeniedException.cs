using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.Base;


namespace BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException;

/// <summary>
/// Ошибка ограничения доступа.
/// </summary>
public class AccessDeniedException : MessageExceptionBase
{
    public AccessDeniedException(string message) 
        : base(message)
    {
    }
}
