using Microsoft.EntityFrameworkCore;

public class CartaRepository{
    private readonly MulliganDeckContext _context;

    public CartaRepository(MulliganDeckContext context){
        _context = context;
    }

    public async Task<List<Carta>> GetCartas(){
        return await _context.Cartas.ToListAsync();
    }

    public async Task<Carta?> GetCartaPorId(int id){
        return await _context.Cartas.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Carta> AddCarta(Carta carta){
        _context.Cartas.Add(carta);
        await _context.SaveChangesAsync();
        return carta;
    }

    public async Task<Carta?> UpdateCarta(int id, Carta carta){
        var existe = await _context.Cartas.AnyAsync(c => c.Id == id);
        if (!existe){
            return null;
        }

        var actualizada = carta with { Id = id };
        _context.Cartas.Update(actualizada);
        await _context.SaveChangesAsync();
        return actualizada;
    }

    public async Task<bool> DeleteCarta(int id){
        var carta = await _context.Cartas.FirstOrDefaultAsync(c => c.Id == id);
        if (carta != null){
            _context.Cartas.Remove(carta);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}