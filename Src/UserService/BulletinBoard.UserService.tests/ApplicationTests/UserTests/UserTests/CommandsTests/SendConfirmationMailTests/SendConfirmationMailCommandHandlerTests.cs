using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.Helpers;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using BulletinBoard.UserService.AppServices.User.User.Commands.SendConfirmationMail;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.User.CommandsTests.SendConfirmationMailTests;

public class SendConfirmationMailCommandHandlerTests
{
    private Mock<ILogger<SendConfirmationMailCommandHandler>> _logger;
    private Mock<UserManager<IdentityUser>> _userManager;
    private Mock<IPublishEndpoint> _publishEndpoint;
    private SendConfirmationMailCommandHandler _handler;
    private CancellationToken _cancellationToken;

    public SendConfirmationMailCommandHandlerTests()
    {
        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();

        _logger = new Mock<ILogger<SendConfirmationMailCommandHandler>>();
        _publishEndpoint = new Mock<IPublishEndpoint>();
        _handler = new SendConfirmationMailCommandHandler(
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
            .Setup(r => r.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
        _userManager.Verify(um => um.FindByIdAsync(command.Id), Times.Once);
    }

    [Fact]
    public async Task GenerateConfirmToken()
    {
        // Arrange 
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser>()), Times.Once);
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
            .Setup(r => r.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(CreateUser());

        _userManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(CreateToken());

        _publishEndpoint.Setup(pe => pe.Publish<UserEmailConfirmationStartedEvent>(It.IsAny<object>(), _cancellationToken))
            .Returns(Task.CompletedTask);
    }

    private SendConfirmationMailCommand CreateCommand()
    {
        var user = CreateUser();
        return new SendConfirmationMailCommand()
        {
           Id = user.Id,
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

    private UserEmailConfirmationStartedEvent CreateEvent(IdentityUser user, string token)
    {
        return new UserEmailConfirmationStartedEvent(user.Id, token);
    }

    private string CreateToken()
    {
        return "SomeToken";
    }
}
