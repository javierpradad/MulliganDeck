using System.Text.Json.Serialization;

namespace MulliganDeck.Infrastructure.Scryfall;

public class ScryfallBulkData
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "";

    [JsonPropertyName("jsonl_download_uri")]
    public string DownloadUri { get; set; } = "";

    [JsonPropertyName("compressed_size")]
    public long CompressedSize { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }
}