namespace BulletinBoard.UserService.AppServices.Common.Exceptions;

/// <summary>
/// Ошибка ненайденной сущности.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}
