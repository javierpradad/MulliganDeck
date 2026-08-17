using MulliganDeck.Domain;

namespace MulliganDeck.Api.Dtos;

public record UpdateDeckDto(string Name, Format Format);