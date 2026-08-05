namespace MulliganDeck.Domain;

public class DeckCard
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int DeckId { get; set; }
    public Deck Deck { get; set; } = null!;

    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;

    public Guid? PrintingId { get; set; }
    public Printing? Printing { get; set; }
}