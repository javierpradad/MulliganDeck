using Microsoft.EntityFrameworkCore;
using MulliganDeck.Domain;

namespace MulliganDeck.Infrastructure;

public class CardRepository{
    private readonly MulliganDeckContext _context;

    public CardRepository(MulliganDeckContext context){
        _context = context;
    }

    public async Task<PagedResult<Card>> GetCards(string? color, string? name, int page = 1, int pageSize = 20){
        var query = _context.Cards.AsQueryable();

        /*if (!string.IsNullOrWhiteSpace(color)){
            query = query.Where(c => c.Colors == color);
        }*/

        if (!string.IsNullOrWhiteSpace(name)){
            query = query.Where(c => c.Name.Contains(name));
        }

        var total = await query.CountAsync();
        var items = await query.OrderBy(c => c.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        var pagedResult = new PagedResult<Card>(items, total, page, pageSize);

        return pagedResult;
    }

    public async Task<Card?> GetCardById(Guid oracleId){
        return await _context.Cards.FirstOrDefaultAsync(c => c.OracleId == oracleId);
    }

}