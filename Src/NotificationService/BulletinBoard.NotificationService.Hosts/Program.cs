using BulletinBoard.Infrastructure.ComponentRegistrar.DbInitializer;
using BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar.Registrar;
using BulletinBoard.NotificationService.Infrastructure.Middleware;
using BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;
using System.Reflection;


namespace BulletinBoard.NotificationService.Hosts;

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
            .RegistrarInitializers();

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.SetSwaggerAuthSettings();

        var app = builder.Build();

        app.UseMiddleware<ExceptionHandlingMiddleware>();


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


