namespace MulliganDeck.Api.Dtos;

public record CardFaceDto(
    string Name,
    string? ImageUri,
    string? TypeLine,
    string? OracleText
);