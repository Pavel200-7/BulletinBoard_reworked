using Microsoft.AspNetCore.Identity;

namespace BulletinBoard.UserService.AppServices.Common.Exceptions.Common;

/// <summary>
/// Ошибки поля.
/// </summary>
public class FieldFailures
{
    public string FieldName { get; set; }
    public List<string> Failures { get; set; }

    public FieldFailures(string fieldName, List<string> fieldFailures)
    {
        FieldName = fieldName;
        Failures = fieldFailures;
    }

    public static List<FieldFailures> FromIdentityErrors(IEnumerable<IdentityError> errors)
    {
         return errors
            .GroupBy(e => e.Code)
            .OrderBy(e => e.Key)
            .Select(e => new FieldFailures(
                e.Key,
                e.Select(e => e.Description).ToList())
            ).ToList();
    }
}