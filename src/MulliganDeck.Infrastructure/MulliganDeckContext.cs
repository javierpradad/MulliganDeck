namespace MulliganDeck.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MulliganDeck.Domain;

public class MulliganDeckContext : DbContext
{
    public MulliganDeckContext(DbContextOptions<MulliganDeckContext> options) : base(options)
    {
    }

    public DbSet<Card> Cartas { get; set; }
}
