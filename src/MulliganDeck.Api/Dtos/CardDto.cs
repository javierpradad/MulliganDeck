using MulliganDeck.Domain;

namespace MulliganDeck.Api.Dtos;

public record CardDto(
    Guid OracleId,
    string Name,
    string OracleText,
    string ManaCost,
    decimal Cmc,
    string Colors,
    string ColorIdentity,
    string TypeLine,
    string? Power,
    string? Toughness
);