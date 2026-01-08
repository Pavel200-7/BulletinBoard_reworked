using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.Base;


namespace BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;

/// <summary>
/// Ошибка ненайденной сущности.
/// </summary>
public class NotFoundException : MessageExceptionBase
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}
