namespace MulliganDeck.Domain;

public class Card
{
    public Guid OracleId { get; set; }
    public required string Name { get; set; }
    public required string TextoOracle { get; set; }
    public required string ManaCost { get; set; }
    public decimal Cmc { get; set; }
    public Color Colors { get; set; }
    public Color ColorIdentity { get; set; }
    public required string TypeLine { get; set; }
    public string? Power { get; set; }
    public string? Toughness { get; set; }
    
    public List<Legality> Legalities { get; set; } = new();
    public List<Keyword> Keywords { get; set; } = new();
    public List<Printing> Printings { get; set; } = new();
    public Guid? DefaultPrintingId { get; set; }
    public Printing? DefaultPrinting { get; set; }
}