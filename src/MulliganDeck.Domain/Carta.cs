namespace MulliganDeck.Domain;

public record Carta(
    int Id,
    string Nombre,
    int CosteMana,
    string Color,
    int Ataque,
    int Vida
);