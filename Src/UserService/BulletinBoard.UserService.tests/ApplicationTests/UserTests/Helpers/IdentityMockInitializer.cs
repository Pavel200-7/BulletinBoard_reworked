using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;


namespace BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.Helpers;

public class IdentityMockInitializer
{
    public Mock<UserManager<TUser>> GetMockUserManager<TUser>()
        where TUser : IdentityUser
    {
        return CreateMockUserManager<TUser>();
    }

    public Mock<SignInManager<TUser>> GetMockSignInManager<TUser>(Mock<UserManager<TUser>> userManager = null)
        where TUser : IdentityUser
    {
        userManager ??= GetMockUserManager<TUser>();

        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var logger = new Mock<ILogger<SignInManager<TUser>>>();
        var schemes = new Mock<IAuthenticationSchemeProvider>();

        return new Mock<SignInManager<TUser>>(
            userManager.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            options.Object,
            logger.Object,
            schemes.Object,
            null 
        );
    }

    private Mock<UserManager<TUser>> CreateMockUserManager<TUser>()
        where TUser : IdentityUser
    {
        var store = new Mock<IUserStore<TUser>>();  
        var options = new Mock<IOptions<IdentityOptions>>();
        options.Setup(o => o.Value).Returns(new IdentityOptions());
        var passwordHasher = new Mock<IPasswordHasher<TUser>>();  
        var userValidators = new List<IUserValidator<TUser>>();  
        var passwordValidators = new List<IPasswordValidator<TUser>>();  
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