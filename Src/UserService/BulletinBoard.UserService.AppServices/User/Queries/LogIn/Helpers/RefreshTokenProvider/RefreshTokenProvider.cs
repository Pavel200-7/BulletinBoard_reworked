using BulletinBoard.UserService.AppServices.Common.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BulletinBoard.UserService.Infrastructure.TokenProviders;

public class RefreshTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser>
    where TUser : IdentityUser
{
    private readonly TimeSpan _tokenLifespan;

    public RefreshTokenProvider(IOptions<RefreshTokenOptions> options)
    {
        _tokenLifespan = options.Value.TokenLifespan;
    }

    public async Task<string> GenerateAsync(
        string purpose,
        UserManager<TUser> manager,
        TUser user)
    {
        // Генерация токена
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "")
            .Replace("+", "")
            .Replace("=", "");

        // Сохраняем токен в хранилище пользователя
        await manager.SetAuthenticationTokenAsync(
            user,
            "RefreshTokenProvider", // имя провайдера
            purpose + user.Id,      // ключ (например: "Refresh_a1b2c3")
            token);

        return token;
    }

    public async Task<bool> ValidateAsync(
        string purpose,
        string token,
        UserManager<TUser> manager,
        TUser user)
    {
        // Получаем сохраненный токен
        var storedToken = await manager.GetAuthenticationTokenAsync(
            user,
            "RefreshTokenProvider",
            purpose + user.Id);

        if (string.IsNullOrEmpty(storedToken))
            return false;

        // Сравниваем токены
        return storedToken == token;
    }

    public Task<bool> CanGenerateTwoFactorTokenAsync(
        UserManager<TUser> manager,
        TUser user)
    {
        // Всегда разрешаем генерацию
        return Task.FromResult(true);
    }
}