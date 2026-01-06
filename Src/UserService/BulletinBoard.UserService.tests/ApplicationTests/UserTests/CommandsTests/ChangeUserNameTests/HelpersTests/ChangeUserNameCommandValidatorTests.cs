using BulletinBoard.UserService.AppServices.User.Commands.ChangeUserName;
using BulletinBoard.UserService.AppServices.User.Commands.ChangeUserName.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.CommandsTests.ChangeUserNameTests.HelpersTests;

public class ChangeUserNameCommandValidatorTests
{
    private ChangeUserNameCommandValidator _validator;

    public ChangeUserNameCommandValidatorTests()
    {
        _validator = new ChangeUserNameCommandValidator();
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

    private ChangeUserNameCommand CreateCommand(
        string id = "SomeId",
        string name = "User12432")
    {
        return new ChangeUserNameCommand()
        {
            Id = id,
            UserName = name
        };
    }
}
