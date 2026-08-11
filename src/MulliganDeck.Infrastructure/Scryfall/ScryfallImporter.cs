using Microsoft.EntityFrameworkCore;
using MulliganDeck.Domain;
using System.IO.Compression;
using System.Text.Json;

namespace MulliganDeck.Infrastructure.Scryfall;

public class ScryfallImporter
{
    private readonly ScryfallClient _client;
    private readonly ScryfallMapper _mapper;
    private readonly MulliganDeckContext _context;
    private readonly IHttpClientFactory _httpFactory;

    public ScryfallImporter(ScryfallClient client, ScryfallMapper mapper, MulliganDeckContext context, IHttpClientFactory httpFactory)
    {
        _client = client;
        _mapper = mapper;
        _context = context;
        _httpFactory = httpFactory;
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

    public async Task<int> ImportBulkAsync()
    {
        var url = await _client.GetOracleCardsUrlAsync();
        if (url == null)
            return 0;

        var existingIds = await _context.Cards
            .Select(c => c.OracleId)
            .ToHashSetAsync();

        var httpClient = _httpFactory.CreateClient("Scryfall");
        using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        await using var compressedStream = await response.Content.ReadAsStreamAsync();
        await using var decompressedStream = new GZipStream(compressedStream, CompressionMode.Decompress);
        using var reader = new StreamReader(decompressedStream);

        var batch = new List<Card>();
        int imported = 0;
        string? line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var scryfallCard = JsonSerializer.Deserialize<ScryfallCard>(line);
            if (scryfallCard == null || existingIds.Contains(scryfallCard.OracleId))
                continue;

            batch.Add(_mapper.ToCard(scryfallCard));
            existingIds.Add(scryfallCard.OracleId);

            if (batch.Count >= 500)
            {
                _context.Cards.AddRange(batch);
                await _context.SaveChangesAsync();
                imported += batch.Count;
                batch.Clear();
            }
        }

        if (batch.Count > 0)
        {
            _context.Cards.AddRange(batch);
            await _context.SaveChangesAsync();
            imported += batch.Count;
        }

        return imported;
    }
}