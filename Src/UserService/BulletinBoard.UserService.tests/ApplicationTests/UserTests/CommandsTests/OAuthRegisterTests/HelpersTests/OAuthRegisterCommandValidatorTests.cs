using BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister;
using BulletinBoard.UserService.AppServices.User.Commands.OAuthRegister.Helpers;

namespace BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.CommandsTests.OAuthRegisterTests.HelpersTests;

public class OAuthRegisterCommandValidatorTests
{
    private readonly OAuthRegisterCommandValidator _validator;

    public OAuthRegisterCommandValidatorTests()
    {
        _validator = new OAuthRegisterCommandValidator();
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
    [InlineData("different state", "state")]
    public void InvalidWhenStatesAreNotTheSame(string state, string expectedState)
    {
        // Arrange 
        var command = CreateCommand(
            state: state,
            expectedState: expectedState);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);

    }

    private OAuthRegisterCommand CreateCommand(
        string provider = "SomeProvider",
        string code = "SomeCode",
        string state = "SomeState",
        string expectedState = "SomeState")
    {
        return new OAuthRegisterCommand()
        {
            Provider = provider,
            Code = code,
            State = state,
            ExpectedState = expectedState
        };

    }
}
