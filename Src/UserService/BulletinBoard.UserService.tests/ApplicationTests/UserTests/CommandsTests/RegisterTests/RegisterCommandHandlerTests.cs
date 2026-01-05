using AutoMapper;
using BulletinBoard.EventBus.Messages.Events.User;
using BulletinBoard.NotificationService.AppServices.Common.Exceptions;
using BulletinBoard.NotificationService.AppServices.User.Commands.Register;
using BulletinBoard.NotificationService.AppServices.User.Enum;
using BulletinBoard.NotificationService.AppServices.User.Repositiry;
using BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.Helpers;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;


namespace BulletinBoard.NotificationService.tests.ApplicationTests.AuthTests.CommandTests.AddUserCommandTests;

public class RegisterCommandHandlerTests
{
    private Mock<ILogger<RegisterCommandHandler>> _logger;
    private Mock<IMapper> _mapper;
    private Mock<UserManager<IdentityUser>> _userManager;
    private Mock<IUserRepository> _userRepository;
    private Mock<IPublishEndpoint> _publishEndpoint;
    private RegisterCommandHandler _handler;
    private CancellationToken _cancellationToken;

    public RegisterCommandHandlerTests()
    {
        var userManagerInitializer = new IdentityMockInitializer();
        _userManager = userManagerInitializer.GetMockUserManager<IdentityUser>();

        _logger = new Mock<ILogger<RegisterCommandHandler>>();
        _mapper = new Mock<IMapper>();
        _userRepository = new Mock<IUserRepository>();
        _publishEndpoint = new Mock<IPublishEndpoint>();
        _handler = new RegisterCommandHandler(
            _logger.Object,
            _mapper.Object, 
            _userManager!.Object, 
            _userRepository.Object,
            _publishEndpoint.Object);
        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task CheckNameForUniqueness()
    {
        // Arrange
        var command = CreateCommand();
        var user = CreateUser();
        _userManager.Setup(um => um.FindByNameAsync(command.UserName))
            .ReturnsAsync(user);

        // Act
        var act = async() => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    [Fact]
    public async Task CheckEmailForUniqueness()
    {
        // Arrange
        var command = CreateCommand();
        var user = CreateUser();
        _userManager.Setup(um => um.FindByEmailAsync(command.Email))
            .ReturnsAsync(user);

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    [Fact]
    public async Task CheckPhoneForUniqueness()
    {
        // Arrange
        var command = CreateCommand();
        var user = CreateUser();

        _userRepository.Setup(r => r.FindByPhoneAsync(command.PhoneNumber, _cancellationToken)) 
            .ReturnsAsync(user);

        // Act
        var act = async () => await _handler.Handle(command, _cancellationToken);

        // Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
    }

    [Fact]
    public async Task RegisterUser()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(u => u.CreateAsync(It.IsAny<IdentityUser>(), command.Password), Times.Once);
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
        _userManager.Verify(u => u.CreateAsync(It.IsAny<IdentityUser>(), command.Password), Times.Once);
    }

    [Fact]
    public async Task AddUserRole()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result =  await _handler.Handle(command, _cancellationToken);

        // Assert
        _userManager.Verify(um => um.AddToRoleAsync(It.IsAny<IdentityUser>(), Roles.User), Times.Once);
    }

    [Fact]
    public async Task ResurnSuccessResponce()
    {
        // Arrange
        var command = CreateCommand();
        var expected = CreateResponce(true);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.Equal(expected.IsSucceed, result.IsSucceed);
    }

    [Fact]
    public async Task PublishUserCreatedEvent()
    {
        // Arrange
        var command = CreateCommand();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        _publishEndpoint.Verify(ep => ep.Publish<UserAddedEvent>(CreateEvent(CreateUser()), _cancellationToken), Times.Once);
    }

    private void SetupMock()
    {
        _userManager
        .Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
        .ReturnsAsync(IdentityResult.Success);

        _userManager
            .Setup(r => r.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        _userManager
            .Setup(r => r.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((IdentityUser)null!);

        _userRepository
            .Setup(r => r.FindByPhoneAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IdentityUser)null!);

        _mapper.Setup(m => m.Map<IdentityUser>(It.IsAny<RegisterCommand>()))
            .Returns(CreateUser());

        _mapper.Setup(m => m.Map<UserAddedEvent>(It.IsAny<IdentityUser>()))
            .Returns(CreateEvent(CreateUser()));

        _publishEndpoint.Setup(pe => pe.Publish<UserAddedEvent>(It.IsAny<Object>(), _cancellationToken))
            .Returns(Task.CompletedTask);
    }

    private RegisterCommand CreateCommand()
    {
        return new RegisterCommand()
        {
            UserName = "User12432",
            Email = "email@email.com",
            PhoneNumber = "+7 (978) 123-45-67",
            Password = "Password123",
            ConfirmPassword = "Password123",
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

    private UserAddedEvent CreateEvent(IdentityUser user)
    {
        return new UserAddedEvent()
        {
            Id = user.Id!,
            UserName = user.UserName!,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!,
        };
    }

    private RegisterCResponse CreateResponce(bool IsSucceed)
    {
        return new RegisterCResponse() { IsSucceed = IsSucceed };
    }
}