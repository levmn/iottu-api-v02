using System.Text;
using DotNetEnv;
using Infrastructure.Iottu.Persistence.Contexts;
using Infrastructure.Iottu.Persistence.Repositories;
using Core.Iottu.Application.Services;
using Core.Iottu.Domain.Interfaces;
using Core.Iottu.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Web.Iottu.Api.Catalog.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

if (!builder.Environment.IsEnvironment("Testing"))
{
    var dbUser = Environment.GetEnvironmentVariable("DB_USER");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    var baseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
    var connectionString = $"User Id={dbUser};Password={dbPassword};{baseConnection}";

    builder.Services.AddDbContext<IottuDbContext>(options =>
        options.UseOracle(connectionString, o => o.MigrationsAssembly("Infrastructure.Iottu.Persistence")));
}

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? throw new InvalidOperationException("Configurações JWT não encontradas");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IMotoRepository, MotoRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IAntenaRepository, AntenaRepository>();
builder.Services.AddScoped<IPatioRepository, PatioRepository>();

builder.Services.AddScoped<IMotoService, MotoService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IAntenaService, AntenaService>();
builder.Services.AddScoped<IPatioService, PatioService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] {} }
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

builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<Infrastructure.Iottu.Persistence.Contexts.IottuDbContext>("Database");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"Iottu API {description.ApiVersion}");
        }

        options.RoutePrefix = string.Empty; // swagger na raiz: http://localhost:5102/
    });
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

public partial class Program { }
