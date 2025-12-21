namespace BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;

/// <summary>
/// Ошибки поля.
/// </summary>
public class FieldFailure
{
    public string FieldName { get; set; }
    public List<string> Failures { get; set; }

    public FieldFailure(string fieldName, List<string> fieldFailures)
    {
        FieldName = fieldName;
        Failures = fieldFailures;
    }
}