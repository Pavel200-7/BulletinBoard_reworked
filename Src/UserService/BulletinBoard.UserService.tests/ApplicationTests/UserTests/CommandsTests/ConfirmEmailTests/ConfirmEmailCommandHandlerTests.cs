using BulletinBoard.NotificationService.AppServices.Common.Exceptions;
using BulletinBoard.NotificationService.AppServices.User.Commands.ConfirmEmail;
using BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;


namespace BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.CommandTests.ConfirmEmailCommandTests;

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

        SetupMock();
    }

    [Fact]
    public async Task ThrowWhenNotFound()
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
    public async Task ConfirmEmailWhenFound()
    {
        //Arrange
        var command = CreateCommand();
        var tokenToInput = Base64UrlEncoder.Decode(command.Token);


        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.ConfirmEmailAsync(It.IsAny<IdentityUser>(), tokenToInput), Times.Once);
    }

    [Fact]
    public async Task ThrowWhenConfirmationFailed()
    {
        // Arrange
        var command = CreateCommand();
        var tokenToInput = Base64UrlEncoder.Decode(command.Token);


        _userManager.Setup(r => r.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Code = "Name", Description = "Something invalid" }));

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
        _userManager.Verify(um => um.ConfirmEmailAsync(It.IsAny<IdentityUser>(), tokenToInput), Times.Once);
    }

    private void SetupMock()
    {
        _userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(CreateUser());

        _userManager.Setup(um => um.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
    }

    private ConfirmEmailCommand CreateCommand()
    {
        var base64EncodedToken = Base64UrlEncoder.Encode("SomeToken");
        return new ConfirmEmailCommand("SomeUserId", base64EncodedToken);
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
