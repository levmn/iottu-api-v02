using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Shared.Iottu.Contracts.DTOs;
using Xunit;

namespace Core.Iottu.Api.IntegrationTests.Controllers;

public class AntenasControllerTests : IntegrationTestBase
{
    private readonly HttpClient _client;

    public AntenasControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateAntena_ShouldReturnCreated()
    {
        await AuthenticateAsync();

        var dto = new CreateAntenaDto
        {
            Localizacao = "Entrada do pátio",
            Identificador = "ANT-01"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/antenas", dto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<AntenaDto>();
        result.Should().NotBeNull();
        result!.Identificador.Should().Be(dto.Identificador);
    }

    [Fact]
    public async Task GetAllAntenas_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync("/api/v1/antenas?page=1&pageSize=10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<object>>();
        result.Should().NotBeNull();
        result!.Items.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateAntena_ShouldReturnBadRequest_WhenModelInvalid()
    {
        await AuthenticateAsync();

        var dto = new CreateAntenaDto(); // campos obrigatórios ausentes

        var response = await _client.PostAsJsonAsync("/api/v1/antenas", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
