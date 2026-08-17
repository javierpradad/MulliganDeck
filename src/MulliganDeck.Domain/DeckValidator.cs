namespace MulliganDeck.Domain;

public class DeckValidator
{
    public ValidationResult Validate(Deck deck)
    { 
        var errors = new List<string>();

        errors.AddRange(CheckDeckSize(deck));
        errors.AddRange(CheckMaxCopies(deck));
        errors.AddRange(CheckColorIdentity(deck));

        return new ValidationResult(errors.Count == 0, errors);
    }

    private List<string> CheckDeckSize(Deck deck)
    {
        var errors = new List<string>();

        var totalCards = deck.Cards.Sum(dc => dc.Quantity);
        var minSize = GetMinimumSize(deck.Format);

        if(deck.Format == Format.Commander && totalCards != minSize)
            errors.Add($"El mazo tiene {totalCards} cartas, pero {deck.Format} requiere exactamente {minSize} cartas");
        else if(totalCards < minSize)
            errors.Add($"El mazo tiene {totalCards} cartas, pero {deck.Format} requiere al menos {minSize} cartas");

        return errors;
    }

    private List<string> CheckMaxCopies(Deck deck)
    {
        var errors = new List<string>();

        var maxCopies = GetMaxCopies(deck.Format);

        foreach(var dc in deck.Cards)
        {
            if(dc.Card.TypeLine.Contains("Basic"))
                continue;

            if(dc.Quantity > maxCopies)
                errors.Add($"El mazo tiene {dc.Quantity} copias de la carta '{dc.Card.Name}', pero {deck.Format} permite un máximo de {maxCopies} copias");
        }

        return errors;
    }

    private List<string> CheckColorIdentity(Deck deck)
    {
        var errors = new List<string>();

        if(deck.Format != Format.Commander)
            return errors;

        if (deck.Commander == null)
        {
            errors.Add("El mazo no tiene un comandante asignado");
            return errors;
        }

        var commanderIdentity = deck.Commander.ColorIdentity;

        foreach(var dc in deck.Cards)
        {
            var cardIdentity = dc.Card.ColorIdentity;

            if((cardIdentity & ~commanderIdentity) != Color.None)
                errors.Add($"{dc.Card.Name} ({cardIdentity}) se sale de la identidad de color del comandante ({commanderIdentity}).");
        }

        return errors;
    }

    private int GetMinimumSize(Format format)
    {
        return format == Format.Commander ? 100 : 60;
    }

    private int GetMaxCopies(Format format)
    {
        return format == Format.Commander ? 1 : 4;
    }
}
