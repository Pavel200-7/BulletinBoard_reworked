using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.Helpers;

public class IdentityMockInitializer
{
    public Mock<UserManager<TUser>> GetMockUserManager<TUser>()
        where TUser : IdentityUser
    {
        return CreateMockUserManager<TUser>();
    }

    public Mock<SignInManager<TUser>> GetMockSignInManager<TUser>(Mock<UserManager<TUser>> userManagerMock = null)
        where TUser : IdentityUser
    {
        userManagerMock ??= GetMockUserManager<TUser>();

        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var logger = new Mock<ILogger<SignInManager<TUser>>>();
        var schemes = new Mock<IAuthenticationSchemeProvider>();

        return new Mock<SignInManager<TUser>>(
            userManagerMock.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            options.Object,
            logger.Object,
            schemes.Object,
            null  // Добавляем седьмой параметр - IUserConfirmation<TUser>
        );
    }

    private Mock<UserManager<TUser>> CreateMockUserManager<TUser>()
        where TUser : IdentityUser
    {
        var store = new Mock<IUserStore<TUser>>();  // Изменили тип
        var options = new Mock<IOptions<IdentityOptions>>();
        options.Setup(o => o.Value).Returns(new IdentityOptions());
        var passwordHasher = new Mock<IPasswordHasher<TUser>>();  // Изменили тип
        var userValidators = new List<IUserValidator<TUser>>();  // Изменили тип
        var passwordValidators = new List<IPasswordValidator<TUser>>();  // Изменили тип
        var normalizer = new Mock<ILookupNormalizer>();
        var errors = new Mock<IdentityErrorDescriber>();
        var services = new Mock<IServiceProvider>();
        var userManagerLogger = new Mock<ILogger<UserManager<TUser>>>();

        return new Mock<UserManager<TUser>>(
            store.Object,
            options.Object,
            passwordHasher.Object,
            userValidators,
            passwordValidators,
            normalizer.Object,
            errors.Object,
            services.Object,
            userManagerLogger.Object
        );
    }
}