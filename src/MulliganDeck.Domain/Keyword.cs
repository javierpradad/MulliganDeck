namespace MulliganDeck.Domain;

public class Keyword
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<Card> Cards { get; set; } = new();
}