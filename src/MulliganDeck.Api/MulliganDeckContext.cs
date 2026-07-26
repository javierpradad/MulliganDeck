using Microsoft.EntityFrameworkCore;

public class MulliganDeckContext : DbContext
{
    public MulliganDeckContext(DbContextOptions<MulliganDeckContext> options) : base(options)
    {
    }

    public DbSet<Carta> Cartas { get; set; }
}
