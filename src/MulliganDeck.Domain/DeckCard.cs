namespace MulliganDeck.Domain;

public class DeckCard
{
    public int Id { get; set; }
    public int Cantidad { get; set; }

    public int DeckId { get; set; }
    public Deck Deck { get; set; } = null!;

    public Guid CartaId { get; set; }
    public Carta Carta { get; set; } = null!;

    public Guid? ImpresionId { get; set; }
    public Impresion? Impresion { get; set; }
}