using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Commands.Register;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using BulletinBoard.UserService.AppServices.User.Enum;


namespace BulletinBoard.UserService.tests.ApplicationTests.AuthTests.CommandTests.AddUserCommandTests;

public class RegisterCommandTests
{
    private Mock<ILogger<RegisterCommandHandler>> MockLogger;
    private Mock<IMapper> MockMapper;
    private Mock<UserManager<IdentityUser>> MockUserManager;
    private Mock<IUserRepository> MockUserRepository;
    private RegisterCommandHandler handler;
    private CancellationToken cancellationToken;

    public RegisterCommandTests()
    {
        MockLogger = new Mock<ILogger<RegisterCommandHandler>>();
        MockMapper = new Mock<IMapper>();
        SetUpUserManager();
        MockUserRepository = new Mock<IUserRepository>();
        handler = new RegisterCommandHandler(MockLogger.Object, MockMapper.Object, MockUserManager!.Object, MockUserRepository.Object);

        cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task CheckNameForUniqueness()
    {
        // Arrange
        var command = CreateCommand();
        var user = CreateUser();
        MockUserManager.Setup(um => um.FindByNameAsync(command.UserName))
            .ReturnsAsync(user);

        // Act
        var act = async() => await handler.Handle(command, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    [Fact]
    public async Task CheckEmailForUniqueness()
    {
        // Arrange
        var command = CreateCommand();
        var user = CreateUser();
        MockUserManager.Setup(um => um.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    [Fact]
    public async Task CheckPhoneForUniqueness()
    {
        // Arrange
        var command = CreateCommand();
        var user = CreateUser();

        MockUserRepository.Setup(r => r.FindByPhoneAsync(command.PhoneNumber, cancellationToken)) 
            .ReturnsAsync(user);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    [Fact]
    public async Task RegisterUser()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        MockUserManager.Verify(u => u.CreateAsync(It.IsAny<IdentityUser>(), command.Password), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_WhenSomethingWrong()
    {
        // Arrange
        var command = CreateCommand();

        MockUserManager
        .Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Code = "Name", Description = "Something invalid" }));

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
        MockUserManager.Verify(u => u.CreateAsync(It.IsAny<IdentityUser>(), command.Password), Times.Once);
    }

    [Fact]
    public async Task AddUserRole()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result =  await handler.Handle(command, cancellationToken);

        // Assert
        MockUserManager.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), Roles.User), Times.Once);
    }

    private void SetupMock()
    {
        MockUserManager
        .Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
        .ReturnsAsync(IdentityResult.Success);

        // Настраиваем проверки уникальности (чтобы они прошли)
        MockUserManager
            .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        MockUserManager
            .Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        // Repository для телефона
        MockUserRepository
            .Setup(r => r.FindByPhoneAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IdentityUser)null!);

        MockMapper.Setup(m => m.Map<IdentityUser>(It.IsAny<RegisterCommand>()))
            .Returns(CreateUser());
        

    }

    private void SetUpUserManager()
    {
        var store = new Mock<IUserStore<IdentityUser>>();

        // Mock IOptions<IdentityOptions>
        var options = new Mock<IOptions<IdentityOptions>>();
        options.Setup(o => o.Value).Returns(new IdentityOptions());

        // Mock IPasswordHasher<IdentityUser>
        var passwordHasher = new Mock<IPasswordHasher<IdentityUser>>();

        // Mock IEnumerable<IUserValidator<IdentityUser>>
        var userValidators = new List<IUserValidator<IdentityUser>>();

        // Mock IEnumerable<IPasswordValidator<IdentityUser>>
        var passwordValidators = new List<IPasswordValidator<IdentityUser>>();

        // Mock ILookupNormalizer
        var normalizer = new Mock<ILookupNormalizer>();

        // Mock IdentityErrorDescriber
        var errors = new Mock<IdentityErrorDescriber>();

        // Mock IServiceProvider
        var services = new Mock<IServiceProvider>();

        // Mock ILogger<UserManager<IdentityUser>>
        var userManagerLogger = new Mock<ILogger<UserManager<IdentityUser>>>();

        // Create the UserManager mock with all required dependencies
        MockUserManager = new Mock<UserManager<IdentityUser>>(
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

    private RegisterCommand CreateCommand()
    {
        return new RegisterCommand()
        {
            UserName = "User12432",
            Email = "email@email.com",
            PhoneNumber = "+7 (978) 123-45-67",
            Password = "Password123",
            ConfirmPassword = "Password123",
        };
    }
    private IdentityUser CreateUser()
    {
        return new IdentityUser()
        {
            UserName = "User12432",
            Email = "email@email.com",
            PhoneNumber = "+7 (978) 123-45-67",
        };
    }
}