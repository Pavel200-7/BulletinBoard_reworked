using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.Base;


namespace BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException;

public class UnauthorizedException : MessageExceptionBase
{
    public UnauthorizedException(string message)
    : base(message)
    {
    }
}
