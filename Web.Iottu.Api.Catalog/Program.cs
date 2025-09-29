using Infrastructure.Iottu.Persistence.Contexts;
using Infrastructure.Iottu.Persistence.Repositories;
using Core.Iottu.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Connection String
var connectionString = builder.Configuration.GetConnectionString("dbIottu");

builder.Services.AddDbContext<IottuDbContext>(options =>
    options.UseOracle(connectionString));

builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IAntenaRepository, AntenaRepository>();

builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
