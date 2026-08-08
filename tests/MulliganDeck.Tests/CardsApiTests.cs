using System.Net;
using System.Net.Http.Json;
using MulliganDeck.Api.Dtos;
using MulliganDeck.Domain;

namespace MulliganDeck.Tests;

public class CardsApiTests : IClassFixture<TestWebAppFactory>
{
    private readonly HttpClient _client;

    public CardsApiTests(TestWebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCards_DevuelveOkConListaVacia()
    {
        var response = await _client.GetAsync("/api/cards");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetCardById_WithNonExistingId_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/cards/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCards_EmptyBase_ReturnsEmptyList()
    {
        var response = await _client.GetAsync("/api/cards");
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PagedResult<CardDto>>();

        Assert.NotNull(result);
        Assert.Equal(0, result.Total);
        Assert.Empty(result.Items);
    }
}