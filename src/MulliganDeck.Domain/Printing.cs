namespace MulliganDeck.Domain;

public class Printing
{
    public Guid Id { get; set; }
    public required string Set { get; set; }
    public required string SetName { get; set; }
    public required string CollectorNumber { get; set; }
    public Rarity Rarity { get; set; }

    public string? ImageUri { get; set; }
    public string? Artist { get; set; }
    public string? FlavorText { get; set; }

    public decimal? PriceEur { get; set; }
    public int? CardmarketId { get; set; }

    public bool Foil { get; set; }
    public bool NonFoil { get; set; }

    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;
}