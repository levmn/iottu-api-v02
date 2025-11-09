using System.Net.Http.Headers;
using System.Net.Http.Json;
using Shared.Iottu.Contracts.DTOs.Auth;
using Xunit;

namespace Core.Iottu.Api.IntegrationTests;

public abstract class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient TestClient;
    protected readonly CustomWebApplicationFactory Factory;

    protected IntegrationTestBase(CustomWebApplicationFactory factory)
    {
        Factory = factory;
        TestClient = factory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        var loginRequest = new LoginRequest
        {
            Username = "admin",
            Password = "admin123"
        };

        var response = await TestClient.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

        TestClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authResponse?.AccessToken);
    }
}