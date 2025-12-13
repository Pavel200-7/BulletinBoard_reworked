using BulletinBoard.Infrastructure.ComponentRegistrar.DbInitializer;
using BulletinBoard.Infrastructure.ComponentRegistrar.Registrar;
using BulletinBoard.Infrastructure.DataAccess.Mapper;
using BulletinBoard.Infrastructure.DataAccess.User.WriteContext;
//using BulletinBoard.UserService.AppServices.Auth.Command.AddUserCommand;
using BulletinBoard.UserService.AppServices.Common.AssembliesNovigation;
using BulletinBoard.UserService.AppServices.Mapper;
//using BulletinBoard.UserService.AppServices.Study;
//using BulletinBoard.UserService.AppServices.Auth.Command.AddUserCommand.Decorators;
//using BulletinBoard.UserService.AppServices.Study.Helpers;
using BulletinBoard.UserService.Hosts.Mapper;
using BulletinBoard.UserService.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;



var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//var assemblies =
// Add services to the container.

builder.Services
    .RegistrarDbContext(configuration);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
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

builder.Services
    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(new[]
        {
            typeof(AssembliesNovigationClass).Assembly, // Hosts
        }))
    .AddAutoMapper(
        typeof(HostMappingProfile),
        typeof(AuthMappingProfile),
        typeof(InfrastructureMappingProfile))
    .RegistrarComponents()
    .RegistrarMethodLogDecorators()
    .RegistrarInfrastructureComponents()
    .RegistrarInitializers();

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.InitAndRunAsync();
