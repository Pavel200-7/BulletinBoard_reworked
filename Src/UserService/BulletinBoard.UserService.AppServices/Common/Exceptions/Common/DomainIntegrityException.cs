using System.Text.Encodings.Web;
using System.Text.Json;

namespace BulletinBoard.UserService.AppServices.Common.Exceptions.Common;

/// <summary>
/// Входные данные не соответствуют правилам домена и их ввод в вистему накушит целостность ее данных.
/// </summary>
public abstract class DomainIntegrityException : Exception
{
    public new string Message { get; set; }
    public List<FieldFailures> FieldsFailures { get; set; }

    public DomainIntegrityException(List<FieldFailures> fieldsFailures, string message)
    {
        FieldsFailures = fieldsFailures;
        Message = message;
    }

    public string ToJsonString()
    {
        var result = new
        {
            message = Message,
            fieldFailures = FieldsFailures.Select(f => new
            {
                field = f.FieldName,
                errors = f.Failures
            })
        };

        return JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
