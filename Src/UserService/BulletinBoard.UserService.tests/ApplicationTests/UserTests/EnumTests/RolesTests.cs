using BulletinBoard.UserService.AppServices.User.Enum;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.EnumTests;

public class RolesTests
{
    [Theory]
    [InlineData("unknownRole")]
    [InlineData("unknownRole2")]
    public void IsRole_WhenUnknown(string roleCandidate)
    {
        // Arrange

        // Act 
        bool isRole = Roles.IsRole(roleCandidate);

        // Assert
        Assert.False(isRole);
    }

    [Theory]
    [InlineData("Admin")]
    [InlineData("User")]
    public void IsRole_WhenExists(string role)
    {
        // Arrange

        // Act 
        bool isRole = Roles.IsRole(role);

        // Assert
        Assert.True(isRole);
    }
}
