namespace MulliganDeck.Domain;

public class CollectionItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public bool Foil { get; set; }

    public Guid PrintingId { get; set; }
    public Printing Printing { get; set; } = null!;
}