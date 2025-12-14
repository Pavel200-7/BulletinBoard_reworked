using BulletinBoard.Infrastructure.ComponentRegistrar.DbInitializer;
using BulletinBoard.Infrastructure.ComponentRegistrar.Registrar;
using BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services
    .RegistrarDbContext(configuration)

    .RegistrarComponents()
    .RegistrarInfrastructureComponents()
    .RegistrarIdentity()
    .AddAutoMapper(Assembly.GetExecutingAssembly()) // Только для текущей сборки
    .RegistrarInitializers();

builder.Services.AddControllers();
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
