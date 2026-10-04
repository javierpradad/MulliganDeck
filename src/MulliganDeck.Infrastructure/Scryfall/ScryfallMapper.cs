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
        var card = new Card
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
            ColorIdentity = ParseColors(source.ColorIdentity),
            Layout = source.Layout,
            ImageUri = source.ImageUris?.Normal
                   ?? source.CardFaces?.ElementAtOrDefault(0)?.ImageUris?.Normal,
        };

        if (source.CardFaces != null)
        {
            foreach (var face in source.CardFaces)
            {
                card.Faces.Add(new CardFace
                {
                    Name = face.Name ?? "",
                    OracleText = face.OracleText,
                    ManaCost = face.ManaCost,
                    TypeLine = face.TypeLine,
                    Power = face.Power,
                    Toughness = face.Toughness,
                    ImageUri = face.ImageUris?.Normal,
                });
            }
        }
        
        return card;
    }
}