using BulletinBoard.UserService.AppServices.Common.Exceptions.Common;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;

namespace BulletinBoard.UserService.AppServices.Common.Exceptions;

public class ValidationException : DomainIntegrityException
{
    public ValidationException(List<FieldFailure> fieldsFailures, string message = nameof(ValidationException)) 
        : base(fieldsFailures, message)
    {
    }
}
