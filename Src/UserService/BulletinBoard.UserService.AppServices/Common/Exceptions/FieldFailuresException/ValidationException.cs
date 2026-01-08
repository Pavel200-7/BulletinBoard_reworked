using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.Base.FieldFailures;


namespace BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException;

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
