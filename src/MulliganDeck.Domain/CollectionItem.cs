namespace MulliganDeck.Domain;

public class CollectionItem
{
    public int Id { get; set; }
    public int Cantidad { get; set; }
    public bool Foil { get; set; }

    public Guid ImpresionId { get; set; }
    public Impresion Impresion { get; set; } = null!;
}