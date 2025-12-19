using AutoMapper;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Commands.Register;
using BulletinBoard.UserService.AppServices.User.Enum;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using BulletinBoard.UserService.tests.ApplicationTests.UserTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace BulletinBoard.UserService.tests.ApplicationTests.AuthTests.CommandTests.AddUserCommandTests;

public class RegisterCommandHandlerTests
{
    private Mock<ILogger<RegisterCommandHandler>> MockLogger;
    private Mock<IMapper> MockMapper;
    private Mock<UserManager<IdentityUser>> MockUserManager;
    private Mock<IUserRepository> MockUserRepository;
    private RegisterCommandHandler handler;
    private CancellationToken cancellationToken;

    public RegisterCommandHandlerTests()
    {
        MockLogger = new Mock<ILogger<RegisterCommandHandler>>();
        MockMapper = new Mock<IMapper>();
        MockUserRepository = new Mock<IUserRepository>();

        var userManagerInitializer = new IdentityMockInitializer();
        MockUserManager = userManagerInitializer.GetMockUserManager<IdentityUser>();

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

    [Fact]
    public async Task ResurnSuccessResponce()
    {
        // Arrange
        var command = CreateCommand();
        var expected = CreateResponce(true);

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        Assert.Equal(expected.IsSucceed, result.IsSucceed);
    }

    private void SetupMock()
    {
        MockUserManager
        .Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
        .ReturnsAsync(IdentityResult.Success);

        MockUserManager
            .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        MockUserManager
            .Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        MockUserRepository
            .Setup(r => r.FindByPhoneAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IdentityUser)null!);

        MockMapper.Setup(m => m.Map<IdentityUser>(It.IsAny<RegisterCommand>()))
            .Returns(CreateUser());
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

    private RegisterResponse CreateResponce(bool IsSucceed)
    {
        return new RegisterResponse() { IsSucceed = IsSucceed };
    }
}