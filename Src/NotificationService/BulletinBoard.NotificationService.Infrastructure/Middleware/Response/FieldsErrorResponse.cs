using BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.Base.FieldFailures;

namespace BulletinBoard.NotificationService.Infrastructure.Middleware.Response;

public class FieldsErrorResponse : BaseErrorResponse
{
    public IEnumerable<FieldFailure> FieldFailures { get; set; }
}
