using BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.Base.FieldFailures;
using System.Diagnostics.CodeAnalysis;


namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;

public static class BusinessRuleIfNotNullExtensions
{
    public static void ThrowBusinessRuleIfNotNull<T>(
    this T? obj,
    List<FieldFailure> fieldsFailures)
    where T : class
    {
        if (obj is not null)
        {
            throw new BusinessRuleException(fieldsFailures);
        }
    }

    public static async Task ThrowBusinessRuleIfNotNull<T>(
        this Task<T?> task,
        List<FieldFailure> fieldsFailures)
        where T : class
    {
        var result = await task.ConfigureAwait(false);
        if (result is not null)
        {
            throw new BusinessRuleException(fieldsFailures);
        }
    }

    public static void ThrowBusinessRuleIfNotNull<T>(
        this T? obj,
        List<FieldFailure> fieldsFailures)
        where T : struct
    {
        if (obj.HasValue)
        {
            throw new BusinessRuleException(fieldsFailures);
        }
    }

    public static async Task ThrowBusinessRuleIfNotNull<T>(
        [NotNull] this Task<T?> task,
        List<FieldFailure> fieldsFailures)
        where T : struct
    {
        var result = await task.ConfigureAwait(false);
        if (result.HasValue)
        {
            throw new BusinessRuleException(fieldsFailures);
        }
    }
}
