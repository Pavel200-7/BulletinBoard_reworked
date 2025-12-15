using BulletinBoard.Infrastructure.ComponentRegistrar.DbInitializer;
using BulletinBoard.Infrastructure.ComponentRegistrar.Registrar;
using BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;
using Microsoft.AspNetCore.Identity;


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
            .RegistrarDbContext(configuration)

            .AddAuthentication(configuration)
            .RegistrarIdentity()
            .RegistrarInitializers();

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.SetSwaggerAuthSettings();

        var app = builder.Build();

        app.MapGroup("/auth").MapIdentityApi<IdentityUser>();

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


