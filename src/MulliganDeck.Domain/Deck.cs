namespace MulliganDeck.Domain;

public class Deck
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public Format Format { get; set; }

    public List<DeckCard> Cards { get; set; } = new();
}