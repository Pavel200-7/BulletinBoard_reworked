namespace BulletinBoard.UserService.Hosts.Registrar;

public static class SessionRegistrar
{
    public static IServiceCollection AddSessionСustom(this IServiceCollection services)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Если будет https заменить None на Always
        });
        services.AddDistributedMemoryCache(); // Кеш для хранения сессий

        return services;
    }
}

