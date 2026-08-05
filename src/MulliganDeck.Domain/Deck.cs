namespace MulliganDeck.Domain;

public class Deck
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public Formato Formato { get; set; }

    public List<DeckCard> Cartas { get; set; } = new();
}