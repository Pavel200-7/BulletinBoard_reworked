using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Commands.ConfirmEmail;
using BulletinBoard.UserService.tests.ApplicationTests.UserTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.CommandTests.ConfirmEmailCommandTests;

public class ConfirmEmailCommandHandlerTests
{
    private Mock<ILogger<ConfirmEmailCommandHandler>> _logger;
    private Mock<UserManager<IdentityUser>> _userManager;
    private ConfirmEmailCommandHandler _handler;
    private CancellationToken _cancellationToken;

    public ConfirmEmailCommandHandlerTests()
    {
        _logger = new Mock<ILogger<ConfirmEmailCommandHandler>>();

        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();
        _handler = new ConfirmEmailCommandHandler(_logger.Object, _userManager.Object);

        _cancellationToken = CancellationToken.None;

        SetUpMock();
    }

    [Fact]
    public async Task MustThrowWhenNotFound()
    {
        //Arrange
        var command = CreateCommand();
        _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
        _userManager.Verify(um => um.FindByIdAsync(command.UserId), Times.Once);
    }

    [Fact]
    public async Task MustConfirmEmailWhenFound()
    {
        //Arrange
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.ConfirmEmailAsync(It.IsAny<IdentityUser>(), command.Token), Times.Once);
    }

    [Fact]
    public async Task MustThrowWhenConfirmationFailed()
    {
        // Arrange
        var command = CreateCommand();

        _userManager.Setup(r => r.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Code = "Name", Description = "Something invalid" }));

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
        _userManager.Verify(um => um.ConfirmEmailAsync(It.IsAny<IdentityUser>(), command.Token), Times.Once);
    }

    private void SetUpMock()
    {
        _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(CreateUser());

        _userManager.Setup(um => um.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
    }

    private ConfirmEmailCommand CreateCommand()
    {
        return new ConfirmEmailCommand("SomeUserId", "SomeToken");
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
