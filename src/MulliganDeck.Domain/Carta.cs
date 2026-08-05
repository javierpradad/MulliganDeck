namespace MulliganDeck.Domain;

public class Carta
{
    public Guid OracleId { get; set; }
    public required string Nombre { get; set; }
    public required string TextoOracle { get; set; }
    public required string CosteMana { get; set; }
    public decimal Cmc { get; set; }
    public Color Colors { get; set; }
    public Color ColorIdentity { get; set; }
    public required string TypeLine { get; set; }
    public string? Poder { get; set; }
    public string? Resistencia { get; set; }
    
    public List<Legalidad> Legalidades { get; set; } = new();
    public List<Keyword> Keywords { get; set; } = new();
    public List<Impresion> Impresiones { get; set; } = new();
    public Guid? ImpresionPredeterminadaId { get; set; }
    public Impresion? ImpresionPredeterminada { get; set; }
}