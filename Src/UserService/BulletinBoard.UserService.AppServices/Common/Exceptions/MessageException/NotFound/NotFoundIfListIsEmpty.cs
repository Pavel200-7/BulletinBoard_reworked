//using System.Threading.Tasks;

//namespace BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;

//public static class NotFoundIfListIsEmpty
//{
//    public static List<T> ThrowNotFoundIfListIsEmpty<T>(this List<T> list, string message)
//        where T : class
//    {
//        if (!list.Any())
//        {
//            throw new NotFoundException(message);
//        }
//        return list;
//    }

//    public static async Task<List<T>> ThrowNotFoundIfListIsEmpty<T>(this Task<List<T>> task, string message)
//        where T : class
//    {
//        var list = await task.ConfigureAwait(false);

//        if (!list.Any())
//        {
//            throw new NotFoundException(message);
//        }
//        return list;
//    }
//}
