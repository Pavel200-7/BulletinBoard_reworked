using BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.Helpers;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.Common.Exceptions.MessageException.NotFound;
using BulletinBoard.UserService.AppServices.User.User.Commands.ResetPassword;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.User.CommandsTests.ResetPasswordTests;

public class ResetPasswordCommandHandlerTests
{
    private Mock<ILogger<ResetPasswordCommandHandler>> _logger;
    private Mock<UserManager<IdentityUser>> _userManager;
    private ResetPasswordCommandHandler _handler;
    private CancellationToken _cancellationToken;

    public ResetPasswordCommandHandlerTests()
    {
        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();

        _logger = new Mock<ILogger<ResetPasswordCommandHandler>>();
        _handler = new ResetPasswordCommandHandler(_logger.Object, _userManager.Object);
        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task CheckEmail()
    {
        // Arrange
        var command = CreateCommand();
        _userManager.Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);


        // Act
        var act = async () =>await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(() => act.Invoke());
        _userManager.Verify(um => um.FindByEmailAsync(command.Email), Times.Once);
    }

    [Fact]
    public async Task ResetPassword()
    {
        // Arrange
        var command = CreateCommand();
        var expectedToken = Base64UrlEncoder.Decode(command.Token);


        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.ResetPasswordAsync(It.Is<IdentityUser>(u => u.Email == command.Email), expectedToken, command.Password), 
            Times.Once);
    }

    [Fact]
    public async Task ThrowWhenResetFailed()
    {
        // Arrange
        var command = CreateCommand();
        _userManager.Setup(r => r.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Code = "Name", Description = "Something invalid" }));

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    [Fact]
    public async Task ResurnSuccessResponce()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.True(result.IsSucceed);
    }

    private void SetupMock()
    {
        _userManager.Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(CreateUser());

        _userManager.Setup(r => r.ResetPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
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

    private ResetPasswordCommand CreateCommand()
    {
        var user = CreateUser();
        var base64EncodedToken = Base64UrlEncoder.Encode("SomeToken");
        return new ResetPasswordCommand()
        {
            Email = user.Email!,
            Token = base64EncodedToken,
            Password = "Password123",
            ConfirmPassword = "Password123"
        };
    }
}
