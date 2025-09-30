using Infrastructure.Iottu.Persistence.Contexts;
using Core.Iottu.Application.Services;
using Core.Iottu.Domain.Interfaces;
using Infrastructure.Iottu.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Web.Iottu.Api.Catalog.Helpers;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var baseConnection = builder.Configuration.GetConnectionString("DefaultConnection");

var connectionString = $"User Id={dbUser};Password={dbPassword};{baseConnection}";

builder.Services.AddDbContext<IottuDbContext>(options =>
    options.UseOracle(connectionString, o => o.MigrationsAssembly("Infrastructure.Iottu.Persistence")));

builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IAntenaService, AntenaService>();
builder.Services.AddScoped<IPatioService, PatioService>();


builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IAntenaRepository, AntenaRepository>();
builder.Services.AddScoped<IPatioRepository, PatioRepository>();

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Iottu API",
        Version = "v1",
        Description = "API RESTful para gestão de Motos Mottu"
    });

    var xmlFiles = new[]
    {
        Path.Combine(AppContext.BaseDirectory, "Web.Iottu.Api.Catalog.xml"),
        Path.Combine(AppContext.BaseDirectory, "Shared.Iottu.Contracts.xml"),
    };
    foreach (var xml in xmlFiles)
    {
        if (File.Exists(xml))
            c.IncludeXmlComments(xml);
    }

    c.SchemaFilter<ExamplesSchemaFilter>();
    c.OperationFilter<QueryParameterOperationFilter>();
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
