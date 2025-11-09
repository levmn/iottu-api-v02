using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Api.IntegrationTests;

public class PatiosControllerTests : IntegrationTestBase
{
    private readonly HttpClient _client;

    public PatiosControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreatePatio_ShouldReturnCreated()
    {
        await AuthenticateAsync();

        var dto = new CreatePatioDto
        {
            Cep = "0100-1000",
            Numero = "123",
            Cidade = "São Paulo",
            Estado = "SP",
            Capacidade = 20
        };

        var response = await _client.PostAsJsonAsync("/api/patios", dto);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<PatioDto>();
        result.Should().NotBeNull();
        result!.Cep.Should().Be(dto.Cep);
    }

    [Fact]
    public async Task GetAllPatios_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/patios?page=1&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().Contain("page");
    }
}
