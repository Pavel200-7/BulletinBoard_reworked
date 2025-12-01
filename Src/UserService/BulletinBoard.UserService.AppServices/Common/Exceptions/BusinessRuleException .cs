using BulletinBoard.UserService.AppServices.Common.Exceptions.Common;


namespace BulletinBoard.UserService.AppServices.Common.Exceptions;

public class BusinessRuleException : DomainIntegrityException
{
    public BusinessRuleException(List<FieldFailures> fieldsFailures, string message) 
        : base(fieldsFailures, message)
    {
    }

    public BusinessRuleException(string fieldName, string falure, string message = "Ошибка бизнес логики")
        : base(GetFalureList(fieldName, falure), message)
    {
    }

    private static List<FieldFailures> GetFalureList(string fieldName, string falure)
    {
        var falures = new List<string>() { falure };
        return new List<FieldFailures>() 
        {
            new FieldFailures(fieldName, falures )
        };
    }
}
