using MulliganDeck.Domain;

namespace MulliganDeck.Tests;

public class DeckValidatorTests
{
    [Fact]
    public void MazoStandard_IsValid()
    {
        var deck = new Deck
        {
            Name = "Test Deck",
            Format = Format.Standard,
            Cards = new List<DeckCard>
            {
                new DeckCard { Card = CrearCarta("Test Card 1"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 2"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 3"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 4"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 5"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 6"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 7"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 8"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 9"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 10"), Quantity = 4 },
                new DeckCard { Card = CrearCarta(typeLine: "Basic Land — Mountain"), Quantity = 20 }
            }
        };

        var validator = new DeckValidator();
        var result = validator.Validate(deck);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void MazoStandard_WithLessThan60Cards_IsInvalid()
    {
        var deck = new Deck
        {
            Name = "Test Deck",
            Format = Format.Standard,
            Cards = new List<DeckCard>
            {
                new DeckCard { Card = CrearCarta(), Quantity = 4 }
            }
        };

        var validator = new DeckValidator();
        var result = validator.Validate(deck);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("al menos"));
    }

    [Fact]
    public void MazoStandard_WithMoreThan4CopiesOfSameCard_IsInvalid()
    {
        var deck = new Deck
        {
            Name = "Test Deck",
            Format = Format.Standard,
            Cards = new List<DeckCard>
            {
                new DeckCard { Card = CrearCarta("Test Card 1"), Quantity = 5 },
                new DeckCard { Card = CrearCarta("Test Card 2"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 3"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 4"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 5"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 6"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 7"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 8"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 9"), Quantity = 4 },
                new DeckCard { Card = CrearCarta("Test Card 10"), Quantity = 4 },
                new DeckCard { Card = CrearCarta(typeLine: "Basic Land — Mountain"), Quantity = 20 }
            }
        };

        var validator = new DeckValidator();
        var result = validator.Validate(deck);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("copias"));
    }

    [Fact]
    public void MazoCommander_IsValid()
    {
        var commander = CrearCarta("Test Commander");

        var deck = new Deck
        {
            Name = "Test Deck",
            Format = Format.Commander,
            Commander = commander,
            Cards = new List<DeckCard>
            {
                new DeckCard { Card = commander, Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 2"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 3"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 4"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 5"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 6"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 7"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 8"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 9"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 10"), Quantity = 1},
                new DeckCard { Card = CrearCarta(typeLine: "Basic Land — Mountain"), Quantity = 90 }
                
            }
        };

        var validator = new DeckValidator();
        var result = validator.Validate(deck);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void MazoCommander_WithLessThan100Cards_IsInvalid()
    {
        var commander = CrearCarta("Test Commander");

        var deck = new Deck
        {
            Name = "Test Deck",
            Format = Format.Commander,
            Commander = commander,
            Cards = new List<DeckCard>
            {
                new DeckCard { Card = commander, Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 2"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 3"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 4"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 5"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 6"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 7"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 8"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 9"), Quantity = 1 },
                new DeckCard { Card = CrearCarta(typeLine: "Basic Land — Mountain"), Quantity = 90 }
            }
        };

        var validator = new DeckValidator();
        var result = validator.Validate(deck);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("exactamente"));
    }

    [Fact]
    public void MazoCommander_WithMoreThan100Cards_IsInvalid()
    {
        var commander = CrearCarta("Test Commander");

        var deck = new Deck
        {
            Name = "Test Deck",
            Format = Format.Commander,
            Commander = commander,
            Cards = new List<DeckCard>
            {
                new DeckCard { Card = commander, Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 2"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 3"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 4"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 5"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 6"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 7"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 8"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 9"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 10"), Quantity = 1},
                new DeckCard { Card = CrearCarta("Test Card 11"), Quantity = 1},
                new DeckCard { Card = CrearCarta(typeLine: "Basic Land — Mountain"), Quantity = 90 }
                
            }
        };

        var validator = new DeckValidator();
        var result = validator.Validate(deck);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("exactamente"));
    }

    [Fact]
    public void MazoCommander_WithMoreThan1Copy_IsInvalid()
    {
        var commander = CrearCarta("Test Commander");

        var deck = new Deck
        {
            Name = "Test Deck",
            Format = Format.Commander,
            Commander = commander,
            Cards = new List<DeckCard>
            {
                new DeckCard { Card = commander, Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 2"), Quantity = 2 },
                new DeckCard { Card = CrearCarta("Test Card 3"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 4"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 5"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 6"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 7"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 8"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 9"), Quantity = 1 },
                new DeckCard { Card = CrearCarta(typeLine: "Basic Land — Mountain"), Quantity = 90 }
                
            }
        };

        var validator = new DeckValidator();
        var result = validator.Validate(deck);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("copias"));
    }

    [Fact]
    public void MazoCommander_WithDifferentColorIdentity_IsInvalid()
    {
        var commander = CrearCarta("Test Commander", Color.Green);

        var deck = new Deck
        {
            Name = "Test Deck",
            Format = Format.Commander,
            Commander = commander,
            Cards = new List<DeckCard>
            {
                new DeckCard { Card = commander, Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 2", Color.Red), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 3"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 4"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 5"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 6"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 7"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 8"), Quantity = 1 },
                new DeckCard { Card = CrearCarta("Test Card 9"), Quantity = 1 },
                new DeckCard { Card = CrearCarta(typeLine: "Basic Land — Mountain"), Quantity = 90 }
                
            }
        };

        var validator = new DeckValidator();
        var result = validator.Validate(deck);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("identidad de color"));
    }

    private Card CrearCarta(string name = "Test Card", Color colorIdentity = Color.None, string typeLine = "Creature — Human")
    {
        return new Card
        {
            OracleId = Guid.NewGuid(),
            Name = name,
            OracleText = "",
            ManaCost = "{1}",
            Cmc = 1,
            Colors = colorIdentity,
            ColorIdentity = colorIdentity,
            TypeLine = typeLine,
            Power = "1",
            Toughness = "1"
        };
    }

    [Fact]
    public void MazoStandard_ConTierrasNevadas_LasTrataComoBasicas()
    {
        var deck = new Deck
        {
            Name = "Test Deck",
            Format = Format.Standard,
            Cards = new List<DeckCard>
            {
                new DeckCard 
                { 
                    Card = CrearCarta(typeLine: "Basic Snow Land — Mountain"), 
                    Quantity = 60 
                }
            }
        };

        var validator = new DeckValidator();
        var result = validator.Validate(deck);

        Assert.True(result.IsValid);
    }
}
