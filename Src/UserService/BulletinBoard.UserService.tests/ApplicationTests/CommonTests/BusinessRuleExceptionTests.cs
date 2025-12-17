using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.Common.Exceptions.Common;


namespace BulletinBoard.UserService.tests.ApplicationTests.CommonTests;

public class BusinessRuleExceptionTests
{
    [Fact]
    public void ToJsonStringIsCorrect()
    {
        // Arrange
        string fildName = "UserName";
        List<string> failures = new List<string>() { "Неправильный формат", "Неверные символы", "Еще какая-нибудь ошибка" };
        var fieldFailures = new FieldFailures(fildName, failures);

        var fieldFailuresList = new List<FieldFailures>() { fieldFailures };
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
