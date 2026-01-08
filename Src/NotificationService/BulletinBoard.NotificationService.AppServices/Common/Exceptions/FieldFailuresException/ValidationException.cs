using BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.Base;
using BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.Base.FieldFailures;


namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException;

/// <summary>
/// Ошибка валидации.
/// </summary>
public class ValidationException : FieldFailuresExceptionBase
{
    public ValidationException(List<FieldFailure> fieldsFailures, string message = nameof(ValidationException)) 
        : base(fieldsFailures, message)
    {
    }
}
