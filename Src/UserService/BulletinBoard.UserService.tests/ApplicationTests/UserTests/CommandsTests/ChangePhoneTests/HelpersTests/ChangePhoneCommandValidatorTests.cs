using BulletinBoard.UserService.AppServices.User.Commands.ChangePhone;
using BulletinBoard.UserService.AppServices.User.Commands.ChangePhone.Helpers;
using BulletinBoard.UserService.AppServices.User.Commands.Register;
using FluentValidation;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.CommandsTests.ChangePhoneTests.HelpersTests;

public class ChangePhoneCommandValidatorTests
{
    private ChangePhoneCommandValidator _validator;

    public ChangePhoneCommandValidatorTests()
    {
        _validator = new ChangePhoneCommandValidator();
    }

    [Fact]
    public void ValidWhenRight()
    {
        // Arrange 
        var command = CreateCommand();

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("9878v9198")]
    [InlineData("243454358399357235")]
    [InlineData("234234")]
    [InlineData("1")]
    public void InvalidWhenPhoneOnIncorrectFormat(string phone)
    {
        // Arrange
        var command = CreateCommand(phone: phone);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("+79261234567")]
    [InlineData("89261234567")]
    [InlineData("79261234567")]
    [InlineData("+7 926 123 45 67")]
    public void ValidPhoneFormat(string phone)
    {
        // Arrange
        var command = CreateCommand(phone: phone);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    private ChangePhoneCommand CreateCommand(
        string id = "SomeId",
        string phone = "+7 (978) 123-45-67")
    {
        return new ChangePhoneCommand()
        {
            Id = id,
            Phone = phone
        };
    }
}
