namespace MulliganDeck.Domain;

public class CardFace
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? OracleText { get; set; }
    public string? ManaCost { get; set; }
    public string? TypeLine { get; set; }
    public string? ImageUri { get; set; }
    public string? Power { get; set; }
    public string? Toughness { get; set; }

    // Relación con la carta
    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;
}