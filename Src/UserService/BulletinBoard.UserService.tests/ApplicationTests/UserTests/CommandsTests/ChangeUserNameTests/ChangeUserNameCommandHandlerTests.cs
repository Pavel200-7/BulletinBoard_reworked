using BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.Helpers;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using BulletinBoard.UserService.AppServices.User.Commands.ChangePhone;
using BulletinBoard.UserService.AppServices.User.Commands.ChangeUserName;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.CommandsTests.ChangeUserNameTests;

public class ChangeUserNameCommandHandlerTests
{
    private Mock<ILogger<ChangeUserNameCommandHandler>> _logger;
    private Mock<UserManager<IdentityUser>> _userManager;
    private ChangeUserNameCommandHandler _handler;
    private CancellationToken _cancellationToken;

    public ChangeUserNameCommandHandlerTests()
    {
        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();

        _logger = new Mock<ILogger<ChangeUserNameCommandHandler>>();
        _handler = new ChangeUserNameCommandHandler(_logger.Object, _userManager!.Object);
        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task CheckNameUnique()
    {
        // Arrange 
        var command = CreateCommand();
        _userManager.Setup(r => r.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(CreateUser());

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
        _userManager.Verify(ur => ur.FindByNameAsync(command.UserName), Times.Once);
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
    public async Task ChangeName()
    {
        // Arrange 
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(ur => ur.SetUserNameAsync(It.IsAny<IdentityUser>(), command.UserName), Times.Once);
    }

    [Fact]
    public async Task ThrowChangePhoneFailed()
    {
        // Arrange 
        var command = CreateCommand();
        _userManager.Setup(ur => ur.SetUserNameAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Code = "Name", Description = "Something invalid" }));

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    private void SetupMock()
    {
        _userManager.Setup(r => r.FindByNameAsync(It.IsAny<string>()))
          .ReturnsAsync((IdentityUser)null!);

        _userManager.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
           .ReturnsAsync(CreateUser());

        _userManager.Setup(um => um.SetUserNameAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
    }

    private ChangeUserNameCommand CreateCommand()
    {
        var user = CreateUser();
        return new ChangeUserNameCommand(user.Id, user.UserName!);
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
}
