using Core.Iottu.Domain.Interfaces;
using Infrastructure.Iottu.Persistence.Contexts;
using Infrastructure.Iottu.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var baseConnection = builder.Configuration.GetConnectionString("DefaultConnection");

var connectionString = $"User Id={dbUser};Password={dbPassword};{baseConnection}";

builder.Services.AddDbContext<IottuDbContext>(options =>
    options.UseOracle(connectionString));

builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IAntenaRepository, AntenaRepository>();

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Iottu API Catalog", 
        Version = "v1",
        Description = "API RESTful para gestão de Motos, Tags e Antenas"
    });

    // Exemplo de schema para Moto
    c.MapType<Shared.Iottu.Contracts.DTOs.MotoDto>(() => new OpenApiSchema
    {
        Type = "object",
        Properties =
        {
            ["id"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("1") },
            ["placa"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("ABC-1234") },
            ["modelo"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Honda CG 160 | Mottu-E") },
            ["cor"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Preta") },
            ["tagId"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("a2fa1f7a-a2bb-471f-a1d0-c453863b5e6e") }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Iottu API v1");
        c.RoutePrefix = string.Empty; // Swagger na raiz: https://localhost:5001/
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
