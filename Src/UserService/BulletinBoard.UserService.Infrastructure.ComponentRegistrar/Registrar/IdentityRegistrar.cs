using BulletinBoard.Infrastructure.DataAccess.User.WriteContext;
using BulletinBoard.UserService.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;

public static class IdentityRegistrar
{
    public static IServiceCollection RegistrarIdentity(this IServiceCollection services)
    {
         services.AddIdentity<ApplicationUser, IdentityRole>(options =>
         {
             // Настройки пароля полностью пустые.
             options.Password.RequiredLength = 1;
             options.Password.RequiredUniqueChars = 0;
             options.Password.RequireNonAlphanumeric = false;
             options.Password.RequireDigit = false;
             options.Password.RequireLowercase = false;
             options.Password.RequireUppercase = false;

             // Настройки блокировки.
             options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
             options.Lockout.MaxFailedAccessAttempts = 5;
             options.Lockout.AllowedForNewUsers = true;
         })
        .AddEntityFrameworkStores<UserContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
