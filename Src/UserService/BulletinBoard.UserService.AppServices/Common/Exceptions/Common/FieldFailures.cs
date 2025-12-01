
using System.Text;

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
}
