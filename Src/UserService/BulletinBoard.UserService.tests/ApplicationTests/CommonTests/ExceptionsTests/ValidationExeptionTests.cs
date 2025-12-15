//using BulletinBoard.UserService.AppServices.Common.Exceptions;
//using BulletinBoard.UserService.AppServices.Common.Exceptions.Common;


//namespace BulletinBoard.UserService.tests.ApplicationTests.CommonTests.ExceptionsTests;

//public class ValidationExeptionTests
//{
//    [Fact]
//    public void ToJsonStringIsCorrect()
//    {
//        // Arrange
//        string fildName = "UserName";
//        List<string> failures = new List<string>() { "Неправильный формат", "Неверные символы", "Еще какая-нибудь ошибка" };
//        var fieldFailures = new FieldFailures(fildName, failures);

//        var fieldFailuresList = new List<FieldFailures>() { fieldFailures };
//        string message = "Ошибка";

//        var exeption = new ValidationException(fieldFailuresList, message);

//        string expected = """
//        {
//          "message": "Ошибка",
//          "fieldFailures": [
//            {
//              "field": "UserName",
//              "errors": [
//                "Неправильный формат",
//                "Неверные символы",
//                "Еще какая-нибудь ошибка"
//              ]
//            }
//          ]
//        }
//        """;

//        // Act
//        var result = exeption.ToJsonString();

//        // Assert
//        Console.WriteLine(expected);
//        Console.WriteLine(result);
//        Assert.Equal(expected, result);
//    }
//}
