using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Shared.Iottu.Contracts.DTOs;
using Xunit;

namespace Core.Iottu.Api.IntegrationTests.Controllers;

public class UsuariosControllerTests : IntegrationTestBase
{
    private readonly HttpClient _client;

    public UsuariosControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateUsuario_ShouldReturnCreated()
    {
        await AuthenticateAsync();

        var dto = new CreateUsuarioDto
        {
            Username = "novo_usuario",
            PasswordHash = "senha123",
            Role = "user"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/usuarios", dto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<UsuarioDto>();
        result.Should().NotBeNull();
        result!.Username.Should().Be(dto.Username);
    }

    [Fact]
    public async Task GetAllUsuarios_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync("/api/v1/usuarios?page=1&pageSize=10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<object>>();
        result.Should().NotBeNull();
        result!.Items.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateUsuario_ShouldReturnBadRequest_WhenModelInvalid()
    {
        await AuthenticateAsync();

        var dto = new CreateUsuarioDto(); // campos obrigatórios ausentes

        var response = await _client.PostAsJsonAsync("/api/v1/usuarios", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
