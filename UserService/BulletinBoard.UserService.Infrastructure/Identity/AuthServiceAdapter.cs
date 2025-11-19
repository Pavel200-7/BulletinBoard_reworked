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

    public AuthServiceAdapter(ILogger<AuthServiceAdapter> logger, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
    }

    public Task<bool> RegisterAsync(UserDto userDto)
    {
        throw new NotImplementedException();
    }
}
