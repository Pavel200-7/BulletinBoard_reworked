using BulletinBoard.NotificationService.AppServices.Common.Configurations;
using BulletinBoard.NotificationService.AppServices.Common.IRepository;
using BulletinBoard.NotificationService.AppServices.Notification.Commands.SendConfirmMail;
using BulletinBoard.NotificationService.AppServices.Notification.Helpers.Mail;
using BulletinBoard.NotificationService.AppServices.Notification.User.Commands.SendResetPasswordMail;
using BulletinBoard.NotificationService.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;


namespace BulletinBoard.NotificationService.tests.ApplicationTests.NotificationTests.UserTests.CommandsTests.SendResetPasswordMailTests;

public class SendResetPasswordMailCommandHandlerTests
{
    private readonly Mock<ILogger<SendResetPasswordMailCommandHandler>> _logger;
    private readonly Mock<IEmailSender> _emailSender;
    private readonly Mock<IQueryRepository<AppUser>> _repository;
    private readonly APIGatewayData _gatewayData;
    private readonly Mock<IOptions<APIGatewayData>> _gatewayDataOptions;
    private readonly SendResetPasswordMailCommandHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public SendResetPasswordMailCommandHandlerTests()
    {
        _logger = new Mock<ILogger<SendResetPasswordMailCommandHandler>>();
        _emailSender = new Mock<IEmailSender>();
        _repository = new Mock<IQueryRepository<AppUser>>();

        _gatewayData = new APIGatewayData()
        {
            Path = "localhost:8080",
        };
        _gatewayDataOptions = new Mock<IOptions<APIGatewayData>>();
        _gatewayDataOptions.Setup(x => x.Value).Returns(_gatewayData);

        _handler = new SendResetPasswordMailCommandHandler(_logger.Object, _emailSender.Object, _repository.Object, _gatewayDataOptions.Object);
        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task MustSendEmailConfirmationMail()
    {
        // Arrange
        var command = CreateCommand();
        var subject = "Сброс пароля.";
        var expectedBody = CreateMailBody();
        var user = CreateUser();

        // Act 
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.True(result.IsSucceed);
        _emailSender.Verify(es => es.SendEmailAsync(
            It.Is<MailData>(md =>
            md.ToEmail == user.Email &&
            md.Subject == subject &&
            md.Message == expectedBody)),
            Times.Once);
    }

    private void SetupMock()
    {
        _emailSender.Setup(es => es.SendEmailAsync(It.IsAny<MailData>()))
            .Returns(Task.CompletedTask);

        _repository.Setup(r => r.GetByIdAsync(CreateUser().Id, _cancellationToken))
            .ReturnsAsync(CreateUser());
    }

    private string CreateMailBody()
    {
        var command = CreateCommand();
        return $"Токен для сброса парося: {command.Token}.";
    }

    private SendResetPasswordMailCommand CreateCommand()
    {
        var user = CreateUser();
        return new SendResetPasswordMailCommand()
        {
            Id = user.Id.ToString(),
            Token = "SomeToken12345"
        };
    }

    private AppUser CreateUser()
    {
        return new AppUser()
        {
            Id = Guid.Parse("b0d4ce5d-2757-4699-948c-cfa72ba94f86"),
            Email = "SomeEmail",
        };
    }
}
