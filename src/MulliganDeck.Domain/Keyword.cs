namespace MulliganDeck.Domain;

public class Keyword
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public List<Carta> Cartas { get; set; } = new();
}