using System.Net.Http.Json;
using FluentAssertions;
using Shared.Iottu.Contracts.DTOs.Auth;
using Xunit;

namespace Core.Iottu.Api.IntegrationTests.Controllers;

public class AuthControllerTests : IntegrationTestBase
{
    public AuthControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Login_ShouldReturnAuthResponse_WhenCredentialsValid()
    {
        var loginRequest = new LoginRequest
        {
            Username = "admin",
            Password = "admin123"
        };

        var response = await TestClient.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        authResponse.Should().NotBeNull();
        authResponse!.AccessToken.Should().NotBeNullOrEmpty();
    }
}