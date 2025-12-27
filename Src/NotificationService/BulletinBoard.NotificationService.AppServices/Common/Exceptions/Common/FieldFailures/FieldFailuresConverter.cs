using FluentValidation.Results;


namespace BulletinBoard.NotificationService.AppServices.Common.Exceptions.Common.FieldFailures;

public static class FieldFailuresConverter
{
    public static List<FieldFailure> FromValidationErrors(IEnumerable<ValidationFailure> errors)
    {
        return errors
           .GroupBy(e => e.PropertyName)
           .OrderBy(e => e.Key)
           .Select(e => new FieldFailure(
               e.Key,
               e.Select(e => e.ErrorMessage).ToList())
           ).ToList(); ;
    }
}
