using MulliganDeck.Domain;

namespace MulliganDeck.Infrastructure.Scryfall;

public class ScryfallMapper
{
    private Color ParseColors(List<string>? colors)
    {
        var result = Color.None;

        if (colors == null)
            return result;

        foreach (var c in colors)
        {
            result |= c switch
            {
                "W" => Color.White,
                "U" => Color.Blue,
                "B" => Color.Black,
                "R" => Color.Red,
                "G" => Color.Green,
                _ => Color.None
            };
        }

        return result;
    }

    public Card ToCard(ScryfallCard source)
    {
        return new Card
        {
            OracleId = source.OracleId,
            Name = source.Name,
            OracleText = source.OracleText ?? "",
            ManaCost = source.ManaCost ?? "",
            Cmc = source.Cmc,
            TypeLine = source.TypeLine ?? "",
            Power = source.Power,
            Toughness = source.Toughness,
            Colors = ParseColors(source.Colors),
            ColorIdentity = ParseColors(source.ColorIdentity)
        };
    }
}