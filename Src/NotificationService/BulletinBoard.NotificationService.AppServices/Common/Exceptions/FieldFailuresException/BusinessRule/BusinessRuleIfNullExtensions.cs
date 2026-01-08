using BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.Base.FieldFailures;
using System.Diagnostics.CodeAnalysis;


namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;

public static class BusinessRuleIfNullExtensions
{
    public static T ThrowBusinessRuleIfNull<T>(
        [NotNull] this T? obj, 
        List<FieldFailure> fieldsFailures)
        where T : class
    {
        if (obj is null)
        {
            throw new BusinessRuleException(fieldsFailures);
        }
        return obj;
    }

    public static async Task<T> ThrowBusinessRuleIfNull<T>(
        [NotNull] this Task<T?> task,
        List<FieldFailure> fieldsFailures)
        where T : class
    {
        var result = await task.ConfigureAwait(false);
        if (result is null)
        {
            throw new BusinessRuleException(fieldsFailures);
        }

        return result;
    }

    public static T ThrowBusinessRuleIfNull<T>(
        [NotNull] this T? obj, 
        List<FieldFailure> fieldsFailures)
        where T : struct
    {
        if (!obj.HasValue)
        {
            throw new BusinessRuleException(fieldsFailures);
        }
        return obj.Value;
    }

    public static async Task<T> ThrowBusinessRuleIfNull<T>(
        [NotNull] this Task<T?> task,
        List<FieldFailure> fieldsFailures)
        where T : struct
    {
        var result = await task.ConfigureAwait(false);
        if (!result.HasValue)
        {
            throw new BusinessRuleException(fieldsFailures);
        }

        return result.Value;
    }
}
