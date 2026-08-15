using MulliganDeck.Domain;

namespace MulliganDeck.Api.Dtos;

public record CreateDeckDto(string Name, Format Format);