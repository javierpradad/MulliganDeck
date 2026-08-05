namespace MulliganDeck.Domain;

public class Legality
{
    public int Id { get; set; }
    public Format Format { get; set; }
    public LegalityStatus Status { get; set; }

    public Guid CardId { get; set; }
    public Card Card { get; set; } = null!;
}