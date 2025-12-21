using BulletinBoard.UserService.AppServices.Common.Exceptions.Common;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common.FieldFailures;


namespace BulletinBoard.UserService.AppServices.Common.Exceptions;

public class BusinessRuleException : DomainIntegrityException
{
    public BusinessRuleException(List<FieldFailure> fieldsFailures, string message = nameof(BusinessRuleException))
        : base(fieldsFailures, message)
    {
    }

    public BusinessRuleException(string fieldName, string falure, string message = nameof(BusinessRuleException))
        : base(GetFalureList(fieldName, falure), message)
    {
    }

    private static List<FieldFailure> GetFalureList(string fieldName, string falure)
    {
        var falures = new List<string>() { falure };
        return new List<FieldFailure>()
        {
            new FieldFailure(fieldName, falures )
        };
    }
}