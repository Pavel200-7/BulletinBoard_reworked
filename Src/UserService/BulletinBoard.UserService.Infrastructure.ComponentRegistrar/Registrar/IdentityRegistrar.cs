using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;


namespace BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;

public static class IdentityRegistrar
{
    public static IServiceCollection RegistrarIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<IdentityUser>(options =>
        {
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.Password.RequiredLength = 10;
            options.Password.RequiredUniqueChars = 3;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;

            // Важно для JWT
            options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
            options.ClaimsIdentity.EmailClaimType = ClaimTypes.Email;
            options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
        })
        .AddEntityFrameworkStores<UserDbContext>()
        .AddApiEndpoints() 
        .AddDefaultTokenProviders();

        return services;
    }
}
