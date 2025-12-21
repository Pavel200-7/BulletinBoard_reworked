using BulletinBoard.Infrastructure.ComponentRegistrar.DbInitializer;
using BulletinBoard.Infrastructure.ComponentRegistrar.Registrar;
using BulletinBoard.UserService.AppServices.Common;
using BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;
using BulletinBoard.UserService.Infrastructure.Middleware;
using Microsoft.AspNetCore.Identity;
using System.Reflection;


namespace BulletinBoard.UserService.Hosts;

public partial class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IConfiguration configuration = builder.Configuration;

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services
            .RegistrarConfigurations(configuration)
            .RegistrarDbContext(configuration)
            .AddAuthentication(configuration)
            .RegistrarComponents()
            .AddAutoMapper(Assembly.GetExecutingAssembly()) 
            .RegistrarIdentity()
            .RegistrarInitializers();

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.SetSwaggerAuthSettings();

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        //app.MapGroup("/auth").MapIdentityApi<IdentityUser>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.InitAndRunAsync();
    }
}


