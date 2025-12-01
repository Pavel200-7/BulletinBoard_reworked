using AutoMapper;
using BulletinBoard.UserService.AppServices.Auth.Enum;
using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter.DTO;
using BulletinBoard.UserService.Infrastructure.Identity;
using BulletinBoard.UserService.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;


namespace BulletinBoard.UserService.tests.InfrastructureTests.IdentityTests;

public class AuthServiceAdapterTests
{
    private Mock<ILogger<AuthServiceAdapter>> logger;
    private Mock<IMapper> mapper;
    private Mock<UserManager<ApplicationUser>> userManager;
    private AuthServiceAdapter authServiceAdapter;
    private CancellationToken cancellationToken;

    public AuthServiceAdapterTests()
    {
        logger = new Mock<ILogger<AuthServiceAdapter>>();
        mapper = new Mock<IMapper>();
        userManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        authServiceAdapter = new(logger.Object, mapper.Object, 
            userManager.Object);

        cancellationToken = CancellationToken.None;

        SetupMock();
    }

    /// <summary>
    /// Должен создавать пользователя через интерфейс identity
    /// </summary>
    [Fact]
    public async Task RegisterAsync_UseUserManagerToCreateUser()
    {
        // Arrange 
        UserCreateDto userCreateDto = CreateValidUserCreateDto();

        // Act
        bool result  = await authServiceAdapter.RegisterAsync(userCreateDto, cancellationToken);

        // Assert
        userManager.Verify(u => u.CreateAsync(It.IsAny<ApplicationUser>(), userCreateDto.Password), Times.Once);
        Assert.True(result);
    }
    /// <summary>
    /// Должен добавлять роль через интерфейс identity
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task AddRole_UseManagerToAddRole()
    {
        // Arrange 
        UserDto userDto = CreateValidUserDto();
        string role = Roles.User;

        // Act
        bool result = await authServiceAdapter.AddRoleByEmailAsync(userDto.Email, role, cancellationToken);

        // Assert
        userManager.Verify(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), role), Times.Once);
        Assert.True(result);
    }

    /// <summary>
    /// Должен выдавать дто пользователя с помощью интерфейса identity
    /// </summary>
    [Fact]
    public async Task GetByLoginAsync_WhenFound()
    { 
        // Arrange
        var userDto = CreateValidUserDto();
        var expectedDto = userDto;

        //Act
        var result = await authServiceAdapter.GetByLoginAsync(userDto.UserName, cancellationToken);

        //Assert
        userManager.Verify(u => u.FindByNameAsync(userDto.UserName), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Email, result.Email);
    }

    /// <summary>
    /// Должен выдавать null когда такого пользователя нет
    /// </summary>
    [Fact]
    public async Task GetByLoginAsync_WhenNotFound()
    {
        // Arrange
        var userDto = CreateValidUserDto();
        ApplicationUser? foundUser = null;
        UserDto? expectedDto = null;

        userManager.Setup(u => u.FindByNameAsync(userDto.UserName))
            .ReturnsAsync(foundUser);

        //Act
        var result = await authServiceAdapter.GetByLoginAsync(userDto.UserName, cancellationToken);

        //Assert
        userManager.Verify(u => u.FindByNameAsync(userDto.UserName), Times.Once);
        Assert.Equal(expectedDto, result);
    }

    /// <summary>
    /// Должен выдавать дто пользователя с помощью интерфейса identity
    /// </summary>
    [Fact]
    public async Task GetByEmailAsync_WhenFound()
    {
        // Arrange
        var userDto = CreateValidUserDto();
        var expectedDto = userDto;

        //Act
        var result = await authServiceAdapter.GetByEmailAsync(userDto.Email, cancellationToken);

        //Assert
        userManager.Verify(u => u.FindByEmailAsync(userDto.Email), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(expectedDto.UserName, result.UserName);
    }

    /// <summary>
    /// Должен выдавать null когда такого пользователя нет
    /// </summary>
    [Fact]
    public async Task GetByEmailAsync_WhenNotFound()
    {
        // Arrange
        var userDto = CreateValidUserDto();
        ApplicationUser? foundUser = null;
        UserDto? expectedDto = null;

        userManager.Setup(u => u.FindByEmailAsync(userDto.Email))
            .ReturnsAsync(foundUser);

        //Act
        var result = await authServiceAdapter.GetByEmailAsync(userDto.Email, cancellationToken);

        //Assert
        userManager.Verify(u => u.FindByEmailAsync(userDto.Email), Times.Once);
        Assert.Equal(expectedDto, result);
    }

    

    private void SetupMock()
    {
        ApplicationUser user = CreateValidApplicationUser();
        mapper.Setup(m => m.Map<ApplicationUser>(It.IsAny<UserCreateDto>()))
            .Returns(user);

        UserDto userDto = CreateValidUserDto();
        mapper.Setup(m => m.Map<UserDto>(It.IsAny<ApplicationUser>()))
            .Returns(userDto);

        userManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        userManager.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        userManager.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(CreateSucceedIdentityResult());

        userManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(CreateSucceedIdentityResult());
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

    private UserDto CreateValidUserDto()
    {
        return new UserDto()
        {
            UserName = "User 1",
            PhoneNumber = "+7 (978) 123-45-67",
            Email = "email@email.com"
        };
    }

    private ApplicationUser CreateValidApplicationUser()
    {
        return new ApplicationUser()
        {
            UserName = "User 1",
            PhoneNumber = "+7 (978) 123-45-67",
            Email = "email@email.com"
        };
    }

    private IdentityResult CreateSucceedIdentityResult()
    {
        return IdentityResult.Success;
    }
}
