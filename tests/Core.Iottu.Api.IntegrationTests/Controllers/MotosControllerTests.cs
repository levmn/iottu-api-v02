using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Api.IntegrationTests;

public class MotosControllerTests : IntegrationTestBase
{
    private readonly HttpClient _client;

    public MotosControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllMotos_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/api/motos?page=1&pageSize=5");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateMoto_ShouldReturnBadRequest_WhenModelInvalid()
    {
        var dto = new CreateMotoDto();

        var response = await _client.PostAsJsonAsync("/api/motos", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
