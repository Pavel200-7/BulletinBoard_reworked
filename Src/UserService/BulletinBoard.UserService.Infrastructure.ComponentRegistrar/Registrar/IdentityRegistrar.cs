using BulletinBoard.NotificationService.AppServices.Common.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;


namespace BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar.Registrar;

public static class IdentityRegistrar
{
    public static IServiceCollection RegistrarIdentity(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.Password.RequiredLength = 10;
            options.Password.RequiredUniqueChars = 3;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;

            options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
            options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
            options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
        })
        .AddEntityFrameworkStores<UserDbContext>()
        .AddApiEndpoints()
        .AddDefaultTokenProviders();

        // Это чтобы выключить редиректы.
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = null;
            options.AccessDeniedPath = null;
            options.LogoutPath = null;

            options.Events = new CookieAuthenticationEvents
            {
                OnRedirectToLogin = context =>
                {
                    throw new UnauthorizedException("Ошибка авторизации.");
                },
                OnRedirectToAccessDenied = context =>
                {
                    throw new AccessDeniedException("Не достаточно прав.");
                },
                OnRedirectToLogout = context =>
                {
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}
