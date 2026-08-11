using System.Net.Http.Json;

namespace MulliganDeck.Infrastructure.Scryfall;

public class ScryfallClient
{
    private readonly HttpClient _http;

    public ScryfallClient(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("Scryfall");
    }

    public async Task<ScryfallCard?> GetCardByNameAsync(string name)
    {
        var response = await _http.GetAsync($"cards/named?exact={Uri.EscapeDataString(name)}");

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ScryfallCard>();
    }

    public async Task<string?> GetOracleCardsUrlAsync()
    {
        var response = await _http.GetAsync("bulk-data");
        if (!response.IsSuccessStatusCode)
            return null;

        var list = await response.Content.ReadFromJsonAsync<ScryfallBulkList>();

        var oracleCards = list?.Data.FirstOrDefault(b => b.Type == "oracle_cards");
        return oracleCards?.DownloadUri;
    }
}