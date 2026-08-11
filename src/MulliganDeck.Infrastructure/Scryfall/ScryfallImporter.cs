using Microsoft.EntityFrameworkCore;
using MulliganDeck.Domain;

namespace MulliganDeck.Infrastructure.Scryfall;

public class ScryfallImporter
{
    private readonly ScryfallClient _client;
    private readonly ScryfallMapper _mapper;
    private readonly MulliganDeckContext _context;

    public ScryfallImporter(ScryfallClient client, ScryfallMapper mapper, MulliganDeckContext context)
    {
        _client = client;
        _mapper = mapper;
        _context = context;
    }

    public async Task<Card?> ImportByNameAsync(string name)
    {
        // 1. Traer de Scryfall
        var scryfallCard = await _client.GetCardByNameAsync(name);
        if (scryfallCard == null)
            return null;

        // 2. ¿Ya existe en la base?
        var existing = await _context.Cards
            .FirstOrDefaultAsync(c => c.OracleId == scryfallCard.OracleId);
        if (existing != null)
            return existing;

        // 3. Mapear y guardar
        var card = _mapper.ToCard(scryfallCard);
        _context.Cards.Add(card);
        await _context.SaveChangesAsync();

        return card;
    }
}