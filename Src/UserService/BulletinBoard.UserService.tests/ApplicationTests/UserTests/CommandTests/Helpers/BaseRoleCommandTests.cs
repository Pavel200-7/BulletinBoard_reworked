using BulletinBoard.UserService.tests.ApplicationTests.UserTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.CommandTests.Helpers;

public abstract class BaseRoleCommandTests
{
    protected Mock<UserManager<IdentityUser>> _userManager;
    protected CancellationToken _cancellationToken;

    public BaseRoleCommandTests()
    {
        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();
        _cancellationToken = CancellationToken.None;
    }

    protected IdentityUser CreateUser()
    {
        return new IdentityUser()
        {
            UserName = "User12432",
            Email = "email@email.com",
            PhoneNumber = "+7 (978) 123-45-67",
        };
    }
}
