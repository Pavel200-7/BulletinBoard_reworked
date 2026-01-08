using BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.Base.FieldFailures;


namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;

public static class BusinessRuleIfFalseExtensions
{
    public static bool ThrowBusinessRuleIfFalse(
        this bool isSucceeded,
        List<FieldFailure> fieldsFailures)
    {
        if (isSucceeded is false)
        {
            throw new BusinessRuleException(fieldsFailures);
        }
        return isSucceeded;
    }
}
