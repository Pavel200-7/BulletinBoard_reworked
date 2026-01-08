using BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.Base.FieldFailures;
using BulletinBoard.NotificationService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;


namespace BulletinBoard.NotificationService.tests.ApplicationTests.CommonTests;

public class BusinessRuleExceptionTests
{
    [Fact]
    public void ToJsonStringIsCorrect()
    {
        // Arrange
        string fildName = "UserName";
        List<string> failures = new List<string>() { "Неправильный формат", "Неверные символы", "Еще какая-нибудь ошибка" };
        var fieldFailures = new FieldFailure(fildName, failures);

        var fieldFailuresList = new List<FieldFailure>() { fieldFailures };
        string message = "Ошибка";

        var exeption = new BusinessRuleException(fieldFailuresList, message);

        string expected = """
        {
          "message": "Ошибка",
          "fieldFailures": [
            {
              "field": "UserName",
              "errors": [
                "Неправильный формат",
                "Неверные символы",
                "Еще какая-нибудь ошибка"
              ]
            }
          ]
        }
        """;

        // Act
        var result = exeption.ToJsonString();

        // Assert
        Assert.Equal(expected, result);
    }
}
