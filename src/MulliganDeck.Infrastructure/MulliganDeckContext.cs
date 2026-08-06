using Microsoft.EntityFrameworkCore;
using MulliganDeck.Domain;

namespace MulliganDeck.Infrastructure;

public class MulliganDeckContext : DbContext
{
    public MulliganDeckContext(DbContextOptions<MulliganDeckContext> options) : base(options)
    {
    }

    public DbSet<Card> Cards { get; set; }
    public DbSet<Printing> Printings { get; set; }
    public DbSet<Deck> Decks { get; set; }
    public DbSet<CollectionItem> CollectionItems { get; set; }
    public DbSet<Keyword> Keywords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Card
        modelBuilder.Entity<Card>()
            .Property(c => c.Colors)
            .HasConversion<string>();
        modelBuilder.Entity<Card>()
            .Property(c => c.ColorIdentity)
            .HasConversion<string>();
        modelBuilder.Entity<Card>()
            .HasMany(c => c.Printings)
            .WithOne(p => p.Card)
            .HasForeignKey(p => p.CardId);
        modelBuilder.Entity<Card>()
            .HasOne(c => c.DefaultPrinting)
            .WithMany()
            .HasForeignKey(c => c.DefaultPrintingId)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Card>()
            .HasKey(c => c.OracleId);
        
        //Printing
        modelBuilder.Entity<Printing>()
            .Property(p => p.Rarity)
            .HasConversion<string>();
        
        //Deck
        modelBuilder.Entity<Deck>()
            .Property(d => d.Format)
            .HasConversion<string>();

        //Legality
        modelBuilder.Entity<Legality>()
            .Property(l => l.Status)
            .HasConversion<string>();
        modelBuilder.Entity<Legality>()
            .Property(l => l.Format)
            .HasConversion<string>();
        modelBuilder.Entity<Legality>()
            .ToTable("Legalities");
    }
}

