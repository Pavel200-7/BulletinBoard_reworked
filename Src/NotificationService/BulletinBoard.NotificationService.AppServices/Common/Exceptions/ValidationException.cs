using BulletinBoard.NotificationService.AppServices.Common.Exceptions.Common;
using BulletinBoard.NotificationService.AppServices.Common.Exceptions.Common.FieldFailures;


namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions;

public class ValidationException : DomainIntegrityException
{
    public ValidationException(List<FieldFailure> fieldsFailures, string message = nameof(ValidationException)) 
        : base(fieldsFailures, message)
    {
    }
}
