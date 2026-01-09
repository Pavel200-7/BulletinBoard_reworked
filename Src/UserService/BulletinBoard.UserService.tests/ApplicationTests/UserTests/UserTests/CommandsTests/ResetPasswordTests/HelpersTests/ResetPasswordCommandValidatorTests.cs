using BulletinBoard.UserService.AppServices.User.User.Commands.ResetPassword;
using BulletinBoard.UserService.AppServices.User.User.Commands.ResetPassword.Helpers;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.User.CommandsTests.ResetPasswordTests.HelpersTests;

public class ResetPasswordCommandValidatorTests
{
    private readonly ResetPasswordCommandValidator _validator;

    public ResetPasswordCommandValidatorTests()
    {
        _validator = new ResetPasswordCommandValidator();
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
    [InlineData("Password123", "Password124")]
    [InlineData("Pass_word123", "Password")]
    public void InvalidWhenPasswordsNotheSame(string password, string confirm)
    {
        // Arrange
        var command = CreateCommand(password: password, confirmPassword: confirm);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    private ResetPasswordCommand CreateCommand(
        string token = "SomeToken",
        string password = "Password123",
        string confirmPassword = "Password123")
    {
        return new ResetPasswordCommand()
        {
            Token = token,
            Password = password,
            ConfirmPassword = confirmPassword
        };

    }
}
