using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Shared.Iottu.Contracts.DTOs;
using Xunit;

namespace Core.Iottu.Api.IntegrationTests.Controllers;

public class TagsControllerTests : IntegrationTestBase
{
    private readonly HttpClient _client;

    public TagsControllerTests(CustomWebApplicationFactory factory) : base(factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateTag_ShouldReturnCreated()
    {
        await AuthenticateAsync();

        var dto = new CreateTagDto
        {
            CodigoRFID = "RFID123",
            SSIDWifi = "WifiTag",
            Latitude = -23.55052,
            Longitude = -46.633308,
            DataHora = DateTime.UtcNow,
            AntenaId = Guid.NewGuid()
        };

        var response = await _client.PostAsJsonAsync("/api/v1/tags", dto);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<TagDto>();
        result.Should().NotBeNull();
        result!.CodigoRFID.Should().Be(dto.CodigoRFID);
    }

    [Fact]
    public async Task GetAllTags_ShouldReturnOk()
    {
        await AuthenticateAsync();

        var response = await _client.GetAsync("/api/v1/tags?page=1&pageSize=10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<PagedResponse<object>>();
        result.Should().NotBeNull();
        result!.Items.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateTag_ShouldReturnBadRequest_WhenModelInvalid()
    {
        await AuthenticateAsync();

        var dto = new CreateTagDto(); // campos obrigatórios ausentes

        var response = await _client.PostAsJsonAsync("/api/v1/tags", dto);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
