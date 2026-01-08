using BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.Helpers;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using BulletinBoard.UserService.AppServices.User.Commands.ChangePhone;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.CommandsTests.ChangePhoneTests;

public class ChangePhoneCommandHandlerTests
{
    private Mock<ILogger<ChangePhoneCommandHandler>> _logger;
    private Mock<UserManager<IdentityUser>> _userManager;
    private Mock<IUserRepository> _userRepository;
    private ChangePhoneCommandHandler _handler;
    private CancellationToken _cancellationToken;

    public ChangePhoneCommandHandlerTests()    
    {
        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();

        _logger = new Mock<ILogger<ChangePhoneCommandHandler>>();
        _userRepository = new Mock<IUserRepository>();
        _handler = new ChangePhoneCommandHandler(_logger.Object, _userManager!.Object, _userRepository.Object);
        _cancellationToken = CancellationToken.None;
        
        SetupMock();
    }

    [Fact]
    public async Task CheckPhoneUnique()
    {
        // Arrange 
        var command = CreateCommand();
        _userRepository.Setup(r => r.FindByPhoneAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(CreateUser());

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
        _userRepository.Verify(ur => ur.FindByPhoneAsync(command.Phone, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CheckUserExist()
    {
        // Arrange 
        var command = CreateCommand();
        _userManager.Setup(r => r.FindByIdAsync(command.Id))
            .ReturnsAsync((IdentityUser)null!);

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
        _userManager.Verify(ur => ur.FindByIdAsync(command.Id), Times.Once);
    }

    [Fact]
    public async Task ChangePhone()
    {
        // Arrange 
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(ur => ur.ChangePhoneNumberAsync(It.IsAny<IdentityUser>(), command.Phone, CreateToken()), Times.Once);
    }

    [Fact]
    public async Task ThrowChangePhoneFailed()
    {
        // Arrange 
        var command = CreateCommand();
        _userManager.Setup(ur => ur.ChangePhoneNumberAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), CreateToken()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Code = "Name", Description = "Something invalid" }));
        // Act
        var act = async() => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    private void SetupMock()
    {
        _userRepository.Setup(r => r.FindByPhoneAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IdentityUser)null!);

        _userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
           .ReturnsAsync(CreateUser());

        _userManager.Setup(um => um.GenerateChangePhoneNumberTokenAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(CreateToken());

        _userManager.Setup(ur => ur.ChangePhoneNumberAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), CreateToken()))
            .ReturnsAsync(IdentityResult.Success);
    }

    private ChangePhoneCommand CreateCommand()
    {
        var user = CreateUser();
        return new ChangePhoneCommand(user.Id, user.PhoneNumber!);
    }

    private IdentityUser CreateUser()
    {
        return new IdentityUser()
        {
            Id = "SomeId",
            UserName = "User12432",
            Email = "email@email.com",
            PhoneNumber = "+7 (978) 123-45-67",
        };
    }

    private string CreateToken()
    {
        return "SomeToken";
    }
}
