using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BulletinBoard.UserService.Hosts;

public static class AuthenticationAdder
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        //var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]!);
        //services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //})
        //.AddBearerToken(IdentityConstants.BearerScheme, options =>
        //{
        //    options.BearerTokenExpiration = TimeSpan.FromHours(1);
        //})
        //.AddJwtBearer(options =>
        //{
        //    options.TokenValidationParameters = new TokenValidationParameters()
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = configuration["JWT:Issuer"],
        //        ValidAudience = configuration["JWT:Audience"],
        //        IssuerSigningKey = new SymmetricSecurityKey(key),

        //    };
        //});

        services.AddAuthentication(IdentityConstants.BearerScheme)
            .AddBearerToken(IdentityConstants.BearerScheme);

        return services;
    }
}
