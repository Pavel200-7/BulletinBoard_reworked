using BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.Base.FieldFailures;


namespace BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;

/// <summary>
/// Ошибка бизнес логики.
/// </summary>
public class BusinessRuleException : FieldFailuresExceptionBase
{
    public BusinessRuleException(List<FieldFailure> fieldsFailures, string message = nameof(BusinessRuleException))
        : base(fieldsFailures, message)
    {
    }
}