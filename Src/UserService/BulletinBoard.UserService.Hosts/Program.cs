using BulletinBoard.Infrastructure.ComponentRegistrar.DbInitializer;
using BulletinBoard.Infrastructure.ComponentRegistrar.Registrar;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.RegistrarInitializers();
builder.Services.RegistrarDbContext(configuration);

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
