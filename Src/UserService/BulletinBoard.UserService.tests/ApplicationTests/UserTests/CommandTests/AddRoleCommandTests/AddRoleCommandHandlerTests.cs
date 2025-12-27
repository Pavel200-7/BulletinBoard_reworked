using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Commands.AddRole;
using BulletinBoard.UserService.AppServices.User.Enum;
using BulletinBoard.UserService.tests.ApplicationTests.UserTests.CommandTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.CommandTests.AddRoleCommandTests;

public class AddRoleCommandHandlerTests : BaseRoleCommandTests
{
    private Mock<ILogger<AddRoleCommandHandler>> _logger;
    private AddRoleCommandHandler _handler;

    public AddRoleCommandHandlerTests()
        : base()
    {
        _logger = new Mock<ILogger<AddRoleCommandHandler>>();
        _handler = new AddRoleCommandHandler(_logger.Object, _userManager!.Object);
        SetupMock();
    }

    [Fact]
    public async Task MustCheckIfUserExists()
    {
        // Arrange 
        var command = CreateCommand();
        _userManager
            .Setup(r => r.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
        _userManager.Verify(um => um.FindByIdAsync(command.UserId), Times.Once);
    }

    [Fact]
    public async Task MustCheckRoleExists()
    {
        // Arrange 
        var command = CreateCommand("UnknownRole");

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    [Fact]
    public async Task MustAddRole()
    {
        // Arrange 
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), command.Role));
    }

    [Fact]
    public async Task MustThrow_WhenAddingFailed()
    {
        // Arrange 
        var command = CreateCommand();
        _userManager.Setup(r => r.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Code = "Name", Description = "Something invalid" }));

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    private void SetupMock()
    {
        _userManager
            .Setup(r => r.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(CreateUser());

        _userManager.Setup(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
    }

    private AddRoleCommand CreateCommand(string role = Roles.Admin)
    {
        return new AddRoleCommand()
        {
            UserId = "userId",
            Role = role
        };
    }
}
