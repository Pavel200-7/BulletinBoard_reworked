using System.Diagnostics.CodeAnalysis;


namespace BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;

public static class NotFoundIfNullExtensions
{
    public static T ThrowNotFoundIfNull<T>([NotNull] this T? obj, string message)
        where T : class
    {
        if (obj is null) 
        {
            throw new NotFoundException(message);
        }
        return obj;
    }

    public static async Task<T> ThrowNotFoundIfNull<T>(
    [NotNull] this Task<T?> task,
    string message)
        where T : class
    {
        var result = await task.ConfigureAwait(false);

        if (result is null)
        {
            throw new NotFoundException(message);
        }

        return result;
    }


    [return: NotNull]
    public static T ThrowNotFoundIfNull<T>([NotNull] this T? obj, string message)
        where T : struct
    {
        if (!obj.HasValue)
        {
            throw new NotFoundException(message);
        }
        return obj.Value;
    }

    public static async Task<T> ThrowNotFoundIfNull<T>(
        [NotNull] this Task<T?> task,
        string message)
        where T : struct
    {
        var result = await task.ConfigureAwait(false);

        if (!result.HasValue)
        {
            throw new NotFoundException(message);
        }

        return result.Value;
    }
}
