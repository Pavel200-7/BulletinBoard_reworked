using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;


namespace BulletinBoard.UserService.Infrastructure.Middleware.Response;

public class FieldsErrorResponse : BaseErrorResponse
{
    public IEnumerable<FieldFailure> FieldFailures { get; set; }
}
