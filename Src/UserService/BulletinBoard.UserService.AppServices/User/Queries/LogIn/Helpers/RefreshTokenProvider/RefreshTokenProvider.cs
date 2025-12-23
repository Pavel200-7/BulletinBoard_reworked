using BulletinBoard.UserService.AppServices.Common.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BulletinBoard.UserService.Infrastructure.TokenProviders;

public class RefreshTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser>
    where TUser : IdentityUser
{
    private readonly TimeSpan _tokenLifespan;

    public RefreshTokenProvider(IOptions<RefreshTokenSettings> options)
    {
        _tokenLifespan = options.Value.TokenLifespan;
    }

    public async Task<string> GenerateAsync(
        string purpose,
        UserManager<TUser> manager,
        TUser user)
    {
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "")
            .Replace("+", "")
            .Replace("=", "");

        await manager.SetAuthenticationTokenAsync(
            user,
            "RefreshTokenProvider", 
            purpose + user.Id,      
            token);

        return token;
    }

    public async Task<bool> ValidateAsync(
        string purpose,
        string token,
        UserManager<TUser> manager,
        TUser user)
    {
        var storedToken = await manager.GetAuthenticationTokenAsync(
            user,
            "RefreshTokenProvider",
            purpose + user.Id);

        if (string.IsNullOrEmpty(storedToken))
            return false;

        return storedToken == token;
    }

    public Task<bool> CanGenerateTwoFactorTokenAsync(
        UserManager<TUser> manager,
        TUser user)
    {
        return Task.FromResult(true);
    }
}