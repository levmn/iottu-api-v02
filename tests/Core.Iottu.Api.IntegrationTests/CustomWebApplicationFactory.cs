using Core.Iottu.Domain.Entities;
using Infrastructure.Iottu.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Core.Iottu.Api.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public FakeAuthContext AuthContext { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<IottuDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<IottuDbContext>(options =>
                options.UseInMemoryDatabase("IntegrationTestsDb"));

            services.AddSingleton(AuthContext);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "TestScheme";
                options.DefaultChallengeScheme = "TestScheme";
            })
            .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>("TestScheme", options => { });

            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IottuDbContext>();
            context.Database.EnsureCreated();

            var hashedPassword = HashPassword("admin123");
            context.Usuarios.Add(new Usuario
            {
                Username = "admin",
                PasswordHash = hashedPassword,
                Role = "admin",
                IsActive = true
            });
            context.SaveChanges();
        });
    }

    private static string HashPassword(string password)
    {
        byte[] salt = Encoding.UTF8.GetBytes("IottuStaticSalt");
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32));
    }
}
