using System.Text.Json.Serialization;

namespace MulliganDeck.Infrastructure.Scryfall;

public class ScryfallImageUris
{
    [JsonPropertyName("small")]
    public string? Small { get; set; }

    [JsonPropertyName("normal")]
    public string? Normal { get; set; }

    [JsonPropertyName("large")]
    public string? Large { get; set; }
}