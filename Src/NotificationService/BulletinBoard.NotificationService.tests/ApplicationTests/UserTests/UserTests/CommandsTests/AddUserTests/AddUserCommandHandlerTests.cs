using AutoMapper;
using BulletinBoard.NotificationService.AppServices.Common.IRepository;
using BulletinBoard.NotificationService.AppServices.User.User.Commands.AddUser;
using BulletinBoard.NotificationService.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace BulletinBoard.NotificationService.tests.ApplicationTests.UserTests.UserTests.CommandsTests.AddUserTests;

public class AddUserCommandHandlerTests
{
    private readonly Mock<ILogger<AddUserCommandHandler>> _logger;
    private readonly Mock<ICommandRepository<AppUser>> _repository;
    private readonly AddUserCommandHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public AddUserCommandHandlerTests()
    {
        _logger = new Mock<ILogger<AddUserCommandHandler>>();
        _repository = new Mock<ICommandRepository<AppUser>>();
        _handler = new AddUserCommandHandler(_logger.Object, _repository.Object);
        _cancellationToken = CancellationToken.None;

        SetupMock();
    }

    [Fact]
    public async Task CreateUser_WhenAllRight()
    {
        // Arrange
        var command = CreateUser();

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.True(result.IsSucceed);
        _repository.Verify(r => r.AddAsync(It.IsAny<AppUser>(), _cancellationToken), Times.Once);
    }

    private AddUserCommand CreateUser()
    {
        return new AddUserCommand()
        {
            Id = Guid.NewGuid().ToString(),
            Email = "SomeEmail"
        };
    }

    private void SetupMock()
    {
    }
}
