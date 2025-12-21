using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;

public static class FieldFailuresConverter
{
    public static List<FieldFailure> FromIdentityErrors(IEnumerable<IdentityError> errors)
    {
        return errors
           .GroupBy(e => e.Code)
           .OrderBy(e => e.Key)
           .Select(e => new FieldFailure(
               e.Key,
               e.Select(e => e.Description).ToList())
           ).ToList();
    }

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
