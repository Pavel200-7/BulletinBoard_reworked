using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.Helpers;
using BulletinBoard.UserService.AppServices.Common.Exceptions.FieldFailuresException.BusinessRule;
using BulletinBoard.UserService.AppServices.User.Helpers.Enum;
using BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister;
using BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister.Helpers;
using BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister.Helpers.OAuth;
using BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister.Helpers.OAuth.Factories;
using BulletinBoard.UserService.AppServices.User.User.Helpers.JWT;
using BulletinBoard.UserService.AppServices.User.User.Helpers.RefreshT;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.UserTests.User.CommandsTests.OAuthRegisterTests;

public class OAuthRegisterCommandHandlerTests
{
    private Mock<ILogger<OAuthRegisterCommandHandler>> _logger;
    private Mock<IMapper> _mapper;
    private Mock<UserManager<IdentityUser>> _userManager;
    private Mock<IOAuthServiceFactory> _oauthServiceFactory;
    private Mock<IOAuthService> _oAuthService;
    private Mock<IJWTProvider> _JWTProvider;
    private Mock<IRefreshTokenProvider> _refreshTProvider;
    private Mock<IPublishEndpoint> _publishEndpoint;
    private OAuthRegisterCommandHandler _handler;
    private CancellationToken _cancellationToken;

    public OAuthRegisterCommandHandlerTests()
    {
        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();

        _logger = new Mock<ILogger<OAuthRegisterCommandHandler>>();
        _mapper = new Mock<IMapper>();
        _oauthServiceFactory = new Mock<IOAuthServiceFactory>();
        _oAuthService = new Mock<IOAuthService>();
        _JWTProvider = new Mock<IJWTProvider>();
        _refreshTProvider = new Mock<IRefreshTokenProvider>();
        _publishEndpoint = new Mock<IPublishEndpoint>();
        _handler = new OAuthRegisterCommandHandler(
            _logger.Object,
            _mapper.Object,
            _userManager!.Object,
            _oauthServiceFactory.Object,
            _JWTProvider.Object,
            _refreshTProvider.Object,
            _publishEndpoint.Object);
        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task GetUserDataFromProvider()
    {
        // Arrange 
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _oAuthService.Verify(oa => oa.GetRegistrationDataFromProviderAsync(command.Code, _cancellationToken));
    }

    [Fact]
    public async Task CheckIsUserExish()
    {
        // Arrange
        var command = CreateCommand();
        var regData = CreateUserRegistrationData();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.FindByEmailAsync(regData.Email), Times.Once);
    }

    [Fact]
    public async Task LoginWhenUserAlreadyExists()
    {
        // Arrange
        var command = CreateCommand();
        var user = CreateUser();
        var expectedTokenData = CreateAccessTokenData();
        var expectedRefreshToken = CreateRefreshToken();

        var regData = CreateUserRegistrationData();
        _userManager
            .Setup(r => r.FindByEmailAsync(regData.Email))
            .ReturnsAsync(CreateUser());

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _JWTProvider.Verify(g => g.GenerateTokenAsync(user.Id, _cancellationToken), Times.Once);
        _refreshTProvider.Verify(rp => rp.GenerateTokenAsync(user.Id, _cancellationToken), Times.Once);

        Assert.Equal(expectedTokenData.TokenType, result.TokenType);
        Assert.Equal(expectedTokenData.AccessToken, result.AccessToken);
        Assert.Equal(expectedTokenData.ExpiresIn, result.ExpiresIn);
        Assert.Equal(expectedRefreshToken, result.RefreshToken);
    }

    [Fact]
    public async Task RegisterUser()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(u => u.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_WhenSomethingWrong()
    {
        // Arrange
        var command = CreateCommand();

        _userManager
        .Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError() { Code = "Name", Description = "Something invalid" }));

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
        _userManager.Verify(u => u.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task AddUserRole()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), Roles.User), Times.Once);
    }

    [Fact]
    public async Task PublishUserCreatedEvent()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _publishEndpoint.Verify(ep => ep.Publish(CreateEvent(CreateUser()), _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task LoginAfterRegistration()
    {
        // Arrange
        var command = CreateCommand();
        var user = CreateUser();
        var expectedTokenData = CreateAccessTokenData();
        var expectedRefreshToken = CreateRefreshToken();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _JWTProvider.Verify(g => g.GenerateTokenAsync(user.Id, _cancellationToken), Times.Once);
        _refreshTProvider.Verify(rp => rp.GenerateTokenAsync(user.Id, _cancellationToken), Times.Once);
        Assert.Equal(expectedTokenData.TokenType, result.TokenType);
        Assert.Equal(expectedTokenData.AccessToken, result.AccessToken);
        Assert.Equal(expectedTokenData.ExpiresIn, result.ExpiresIn);
        Assert.Equal(expectedRefreshToken, result.RefreshToken);
    }

    private void SetupMock()
    {
        _oauthServiceFactory.Setup(asf => asf.CreateOAuthService(It.IsAny<string>()))
            .Returns(_oAuthService.Object);

        _oAuthService
            .Setup(oa => oa.GetRegistrationDataFromProviderAsync(It.IsAny<string>(), _cancellationToken))
            .ReturnsAsync(CreateUserRegistrationData());

        var user = CreateUser();
        _userManager
            .Setup(r => r.FindByEmailAsync(user.Email!))
            .ReturnsAsync((IdentityUser)null!);

        _JWTProvider.Setup(g => g.GenerateTokenAsync(user.Id, _cancellationToken))
            .ReturnsAsync(CreateAccessTokenData());

        _refreshTProvider.Setup(rp => rp.GenerateTokenAsync(user.Id, _cancellationToken))
            .ReturnsAsync(CreateRefreshToken());

        _userManager
            .Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _mapper.Setup(m => m.Map<UserAddedEvent>(It.IsAny<IdentityUser>()))
            .Returns(CreateEvent(user));

        _mapper.Setup(m => m.Map<IdentityUser>(It.IsAny<PartialUser>()))
            .Returns(user);

        _publishEndpoint.Setup(pe => pe.Publish<UserAddedEvent>(It.IsAny<object>(), _cancellationToken))
            .Returns(Task.CompletedTask); 
    }

    private OAuthRegisterCommand CreateCommand()
    {
        return new OAuthRegisterCommand()
        {
            Provider = "github",
            Code = "SomeToken",
            State = "SomeState",
            ExpectedState = "SomeState"
        };
    }

    private UserRegistrationData CreateUserRegistrationData()
    {
        return new UserRegistrationData("email@email.com");
    }

    private PartialUser CreatePartialUserUser()
    {
        var userRegData = CreateUserRegistrationData();
        return new PartialUser(userRegData.Email);
    }

    private IdentityUser CreateUser()
    {
        var partialUser = CreatePartialUserUser();
        return new IdentityUser()
        {
            Id = "SomeId",
            UserName = partialUser.UserName,
            Email = partialUser.Email,
            PhoneNumber = partialUser.PhoneNumber,
        };
    }

    private UserAddedEvent CreateEvent(IdentityUser user)
    {
        return new UserAddedEvent()
        {
            Id = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!,
        };
    }

    private TokenData CreateAccessTokenData()
    {
        return new TokenData()
        {
            TokenType = "Bearer",
            AccessToken = "mocked-access-token-jwt-string",
            ExpiresIn = 3600
        };
    }

    private string CreateRefreshToken()
    {
        return "mocked-refresh-token-12345";
    }
}
