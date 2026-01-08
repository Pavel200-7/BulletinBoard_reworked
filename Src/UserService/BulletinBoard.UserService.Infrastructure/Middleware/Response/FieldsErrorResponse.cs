using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.Base.FieldFailures;


namespace BulletinBoard.UserService.Infrastructure.Middleware.Response;

public class FieldsErrorResponse : BaseErrorResponse
{
    public IEnumerable<FieldFailure> FieldFailures { get; set; }
}
