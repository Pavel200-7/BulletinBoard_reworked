using AutoMapper;
using BulletinBoard.UserService.AppServices.Auth.Command.AddUserCommand;
using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter;
using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter.DTO;
using BulletinBoard.UserService.AppServices.Common.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;


namespace BulletinBoard.UserService.tests.ApplicationTests.AuthTests.CommandTests.AddUserCommandTests;

public class AddUserCommandHandlerTests
{
    private Mock<ILogger<AddUserCommandHandler>> logger;
    private Mock<IMapper> mapper;
    private Mock<IValidator<AddUserCommand>> validator;
    private Mock<IAuthServiceAdapter> authServiceAdapter;
    private AddUserCommandHandler handler;
    private CancellationToken cancellationToken;

    public AddUserCommandHandlerTests()
    {
        logger = new Mock<ILogger<AddUserCommandHandler>>();
        mapper = new Mock<IMapper>();
        validator = new Mock<IValidator<AddUserCommand>>();
        authServiceAdapter = new Mock<IAuthServiceAdapter>();
        handler = new AddUserCommandHandler(logger.Object, mapper.Object, validator.Object, authServiceAdapter.Object);

        cancellationToken = CancellationToken.None;

        SetupMock();
    }

    /// <summary>
    /// Должен проводить валидацию
    /// </summary>
    [Fact]
    public async Task Handle_WhenCommandValid_ValidatesCommand()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var response = await handler.Handle(command, cancellationToken);

        // Assert
        validator.Verify(v => v.ValidateAsync(command, cancellationToken), Times.Once);
    }

    /// <summary>
    /// Должен выбрасывать ошибку валидации при неверных данных
    /// </summary>
    [Fact]
    public async Task Handle_WhenValidationFails_ThrowsValidationException()
    {
        // Arrange
        var command = CreateInvalidCommand();

        List<ValidationFailure> validationFailures = new List<ValidationFailure>()
        {
            new ValidationFailure("PhoneNumber", "Неправильный формат телефона")
        };

        validator.Setup(v => v.ValidateAsync(command, cancellationToken))
             .ReturnsAsync(new ValidationResult(validationFailures));

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<AppServices.Common.Exceptions.ValidationException>(() => act.Invoke());
        validator.Verify(validator => validator.ValidateAsync(command, cancellationToken), Times.Once);
    }

    /// <summary>
    /// Должен проверять логин
    /// </summary>
    [Fact]
    public async Task Handle_WhenUserNameExists_ThrowsBusinessRuleException()
    {
        // Arrange
        var command = CreateValidCommand();

        var userDto = CreateValidUserDto();
        authServiceAdapter.Setup(a => a.GetByLoginAsync(command.UserName, cancellationToken))
            .ReturnsAsync(userDto);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        //Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
        authServiceAdapter.Verify(a => a.GetByLoginAsync(command.UserName, cancellationToken), Times.Once);
    }

    /// <summary>
    /// Должен проверять почту
    /// </summary>
    [Fact]
    public async Task Handle_WhenEmailExists_ThrowsBusinessRuleException()
    {
        // Arrange
        var command = CreateValidCommand();

        var expectedResponse = new AddUserResponse() { IsSucceed = true };

        var userDto = CreateValidUserDto();
        authServiceAdapter.Setup(a => a.GetByEmailAsync(command.Email, cancellationToken))
            .ReturnsAsync(userDto);

        // Act
        var act = async () => await handler.Handle(command, cancellationToken);

        //Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => act.Invoke());
        authServiceAdapter.Verify(a => a.GetByEmailAsync(command.Email, cancellationToken), Times.Once);
    }

    /// <summary>
    /// Должен вносить данные нового пользователя в БД
    /// </summary>
    [Fact]
    public async Task Handle_WhenEverythingOk_CreateUserAccount()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Arrange
        authServiceAdapter.Verify(a => a.RegisterAsync(It.IsAny<UserCreateDto>(), cancellationToken), Times.Once);
    }

    /// <summary>
    /// Должен мапить соманду в дто создания
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Handle_WhenEverythingOk_MapsCommandToUserCreateDto()
    {
        // Arrange
        var command = CreateValidCommand();

        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        mapper.Verify(m => m.Map<UserCreateDto>(command), Times.Once);
    }

    /// <summary>
    /// Должен вносить возвращать положительный ответ заданного формата когда все хорошо
    /// </summary>
    [Fact]
    public async Task Handle_WhenEverythingOk_ReturnSuccessResponce()
    {
        // Arrange
        var command = CreateValidCommand();
        bool SucceedExpected = true;
        // Act
        var result = await handler.Handle(command, cancellationToken);

        // Assert
        Assert.Equal(SucceedExpected, result.IsSucceed);
    }

    private void SetupMock()
    {
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<AddUserCommand>(), cancellationToken))
            .ReturnsAsync(new ValidationResult());

        UserDto? userDto = null;
        authServiceAdapter.Setup(a => a.GetByLoginAsync(It.IsAny<string>(), cancellationToken))
            .ReturnsAsync(userDto);

        authServiceAdapter.Setup(a => a.GetByEmailAsync(It.IsAny<string>(), cancellationToken))
            .ReturnsAsync(userDto);

        UserCreateDto userCreateDto = CreateValidUserCreateDto();
        mapper.Setup(m => m.Map<UserCreateDto>(It.IsAny<AddUserCommand>()))
            .Returns(userCreateDto);

        authServiceAdapter
            .Setup(a => a.RegisterAsync(It.IsAny<UserCreateDto>(), cancellationToken))
            .ReturnsAsync(true);

    }

    private AddUserCommand CreateValidCommand()
    {
        return new AddUserCommand()
        {
            UserName = "User 1",
            PhoneNumber = "+7 (978) 123-45-67",
            Email = "email@email.com",
            Password = "Password123",
            ConfirmPassword = "Password123",
        };
    }

    private AddUserCommand CreateInvalidCommand()
    {
        return new AddUserCommand()
        {
            UserName = "User 1",
            PhoneNumber = "+7 (978) 123sef5-sgv",
            Email = "email@email.com",
            Password = "Password123",
            ConfirmPassword = "Password123",
        };
    }

    private UserDto CreateValidUserDto()
    {
        return new UserDto()
        {
            UserName = "User 1",
            PhoneNumber = "+7 (978) 123-45-67",
            Email = "email@email.com",
        };
    }

    private UserCreateDto CreateValidUserCreateDto()
    {
        return new UserCreateDto()
        {
            UserName = "User 1",
            PhoneNumber = "+7 (978) 123-45-67",
            Email = "email@email.com",
            Password = "Password123",
        };
    }
}
