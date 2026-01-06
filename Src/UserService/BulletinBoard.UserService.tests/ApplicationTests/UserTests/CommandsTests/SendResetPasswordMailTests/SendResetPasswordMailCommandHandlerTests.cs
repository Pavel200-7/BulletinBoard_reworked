using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.Helpers;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using BulletinBoard.UserService.AppServices.User.Commands.SendResetPasswordMail;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.CommandsTests.SendPasswordChangeMailTests;

public class SendResetPasswordMailCommandHandlerTests
{
    private Mock<ILogger<SendResetPasswordMailCommandHandler>> _logger;
    private Mock<UserManager<IdentityUser>> _userManager;
    private Mock<IPublishEndpoint> _publishEndpoint;
    private SendResetPasswordMailCommandHandler _handler;
    private CancellationToken _cancellationToken;

    public SendResetPasswordMailCommandHandlerTests()
    {
        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();

        _logger = new Mock<ILogger<SendResetPasswordMailCommandHandler>>();
        _publishEndpoint = new Mock<IPublishEndpoint>();
        _handler = new SendResetPasswordMailCommandHandler(
            _logger.Object,
            _userManager.Object,
            _publishEndpoint.Object);
        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task CheckIfUserExists()
    {
        // Arrange 
        var command = CreateCommand();
        _userManager
            .Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
        _userManager.Verify(um => um.FindByEmailAsync(command.Email), Times.Once);
    }

    [Fact]
    public async Task GenerateConfirmToken()
    {
        // Arrange 
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()), Times.Once);
    }

    [Fact]
    public async Task PublishUserCreatedEvent()
    {
        // Arrange
        var command = CreateCommand();
        var expectedTokenToPutInEvent = Base64UrlEncoder.Encode(CreateToken());
        var expectedPublishedEvent = CreateEvent(CreateUser(), expectedTokenToPutInEvent);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _publishEndpoint.Verify(ep => ep.Publish(expectedPublishedEvent, _cancellationToken), Times.Once);
    }


    private void SetupMock()
    {
        _userManager
            .Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(CreateUser());

        _userManager.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(CreateToken());

        _publishEndpoint.Setup(pe => pe.Publish<UserResetPasswordStartedEvent>(It.IsAny<Object>(), _cancellationToken))
            .Returns(Task.CompletedTask);
    }

    private SendResetPasswordMailCommand CreateCommand()
    {
        var user = CreateUser();
        return new SendResetPasswordMailCommand()
        {
            Email = user.Email!,
        };
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

    private UserResetPasswordStartedEvent CreateEvent(IdentityUser user, string token)
    {
        return new UserResetPasswordStartedEvent(user.Id, token);
    }

    private string CreateToken()
    {
        return "SomeToken";
    }
}
