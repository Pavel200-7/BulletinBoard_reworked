using BulletinBoard.UserService.AppServices.User.Helpers.Enum;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.HelpersTests.EnumTests;

public class RolesTests
{
    [Theory]
    [InlineData("unknownRole")]
    [InlineData("unknownRole2")]
    public void IsRoleWhenUnknown(string roleCandidate)
    {
        // Act 
        bool isRole = Roles.IsRole(roleCandidate);

        // Assert
        Assert.False(isRole);
    }

    [Theory]
    [InlineData(Roles.Admin)]
    [InlineData(Roles.User)]
    public void IsRoleWhenExists(string role)
    {
        // Act 
        bool isRole = Roles.IsRole(role);

        // Assert
        Assert.True(isRole);
    }
}
