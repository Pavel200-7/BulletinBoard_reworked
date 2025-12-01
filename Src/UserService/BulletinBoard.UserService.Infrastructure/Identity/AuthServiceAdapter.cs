using AutoMapper;
using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter;
using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter.DTO;
using BulletinBoard.UserService.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BulletinBoard.UserService.Infrastructure.Identity;

public class AuthServiceAdapter : IAuthServiceAdapter
{
    private ILogger<AuthServiceAdapter> _logger;
    private IMapper _mapper;
    private UserManager<ApplicationUser> _userManager;

    public AuthServiceAdapter(
        ILogger<AuthServiceAdapter> logger, 
        IMapper mapper, 
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<bool> RegisterAsync(UserCreateDto userDto, CancellationToken cancellationToken)
    {
        ApplicationUser user = _mapper.Map<ApplicationUser>(userDto);
        var result = await _userManager.CreateAsync(user, userDto.Password);
        return result.Succeeded;
    }

    public async Task<UserDto?> GetByLoginAsync(string login, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(login);
        return user is not null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);
        return user is not null ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<bool> AddRoleByEmailAsync(string email, string role, CancellationToken cancellationToken)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);
        if (user is  null) return false;
        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }
}
