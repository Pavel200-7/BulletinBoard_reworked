using BulletinBoard.UserService.AppServices.User.Commands.Register;
using BulletinBoard.UserService.AppServices.User.Commands.Register.Helpers;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.CommandTests.RegisterCommandTests;

public class RegisterCommandValidatorTests
{
    private RegisterCommandValidator _validator;

    public RegisterCommandValidatorTests()
    {
        _validator = new RegisterCommandValidator();
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

    [Fact]
    public void InvalidWhenNameIsEmpty()
    {
        // Arrange
        var command = CreateCommand(name: "");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("small")]
    [InlineData("vvvvvveeeeeerrrrrryyyyyyLLLLLLaaaaaarrrrrrggggggeeeeee")]
    public void InvalidWhenNameLessThen8Char(string name)
    {
        // Arrange
        var command = CreateCommand(name: name);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }


    /// <summary>
    /// Под запрещенными имеются ввиду все кроме русских, английских букв, цифр и знака _.
    /// </summary>
    [Theory]
    [InlineData("NormalNameWith*")]
    [InlineData("NormalNameWith$")]
    [InlineData("NormalNameWith{}")]
    [InlineData("NormalNameWith (space and)")]
    public void InvalidWhenNameHasRestrictSymbols(string name)
    {
        // Arrange
        var command = CreateCommand(name: name);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("D322434")]
    [InlineData("3224324sfd4")]

    public void InvalidWhenNameHasLessThan6Symbols(string name)
    {
        // Arrange
        var command = CreateCommand(name: name);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("3224324dszffsdfbfsfd4")]
    [InlineData("322NormalNameWith")]
    public void InvalidWhenNameStartsNotWithLetter(string name)
    {
        // Arrange
        var command = CreateCommand(name: name);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("NormalNameWith1314")]
    [InlineData("No123rmalNa234meWith_324325")]
    public void ValidName(string name)
    {
        // Arrange
        var command = CreateCommand(name: name);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("emailincorrectformat1434")]
    [InlineData("emailin@correctformat143@4")]
    public void InvalidWhenEmailOnIncorrectFormat(string email)
    {
        // Arrange
        var command = CreateCommand(email: email);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
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


    private RegisterCommand CreateCommand(
        string name = "User12432", 
        string email = "Email@email.com", 
        string phone = "+7 (978) 123-45-67", 
        string password = "Password123",
        string confirmPassword = "Password123")
    {
        return new RegisterCommand()
        { 
            UserName = name,
            Email = email,
            PhoneNumber = phone,
            Password = password,
            ConfirmPassword = confirmPassword
        };
    }
}
