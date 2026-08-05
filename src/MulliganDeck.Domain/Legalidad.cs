namespace MulliganDeck.Domain;

public class Legalidad
{
    public int Id { get; set; }
    public Formato Formato { get; set; }
    public EstadoLegalidad Estado { get; set; }

    public Guid CartaId { get; set; }
    public Carta Carta { get; set; } = null!;
}