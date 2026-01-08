using BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.CommandTests.Helpers;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using BulletinBoard.UserService.AppServices.User.Commands.AddRole;
using BulletinBoard.UserService.AppServices.User.Commands.DeleteRole;
using BulletinBoard.UserService.AppServices.User.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;


namespace BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.CommandTests.DeleteRoleCommandTests;

public class DeleteRoleCommandHandlerTests : BaseRoleCommandTests
{
    private Mock<ILogger<AddRoleCommandHandler>> _logger;
    private DeleteRoleCommandHandler _handler;

    public DeleteRoleCommandHandlerTests()
    {
        _logger = new Mock<ILogger<AddRoleCommandHandler>>();
        _handler = new DeleteRoleCommandHandler(_logger.Object, _userManager!.Object);
        SetupMock();
    }

    [Fact]
    public async Task CheckIfUserExists()
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
    public async Task CheckIfRoleExists()
    {
        // Arrange 
        var command = CreateCommand("UnknownRole");

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    [Fact]
    public async Task DeleteRole()
    {
        // Arrange 
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.RemoveFromRoleAsync(It.IsAny<IdentityUser>(), command.Role));
    }

    [Fact]
    public async Task ThrowWhenAddingFailed()
    {
        // Arrange 
        var command = CreateCommand();
        _userManager.Setup(r => r.RemoveFromRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
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

        _userManager.Setup(um => um.RemoveFromRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
    }

    private DeleteRoleCommand CreateCommand(string role = Roles.Admin)
    {
        return new DeleteRoleCommand()
        {
            UserId = "userId",
            Role = role
        };
    }
}
