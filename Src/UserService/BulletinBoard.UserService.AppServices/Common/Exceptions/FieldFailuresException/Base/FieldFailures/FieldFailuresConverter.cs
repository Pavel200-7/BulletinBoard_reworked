using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.Base.FieldFailures;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;


namespace BulletinBoard.UserService.AppServices.Common.Exceptions.DomainIntegrityException.Base.FieldFailures;

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

    public static List<FieldFailure> FromSingleFieldError(string fieldName, string falure)
    {
        var falures = new List<string>() { falure };
        return new List<FieldFailure>()
        {
            new FieldFailure(fieldName, falures )
        };
    }
}
