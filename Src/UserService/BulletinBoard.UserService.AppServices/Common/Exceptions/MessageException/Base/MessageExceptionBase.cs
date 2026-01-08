namespace BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.Base;

public abstract class MessageExceptionBase : Exception
{
    public MessageExceptionBase(string message)
        : base(message)
    {
    }
}
