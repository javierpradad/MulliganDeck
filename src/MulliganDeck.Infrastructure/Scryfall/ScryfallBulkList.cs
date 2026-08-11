using System.Text.Json.Serialization;

namespace MulliganDeck.Infrastructure.Scryfall;

public class ScryfallBulkList
{
    [JsonPropertyName("data")]
    public List<ScryfallBulkData> Data { get; set; } = new();
}