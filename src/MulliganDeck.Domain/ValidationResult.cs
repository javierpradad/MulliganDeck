namespace MulliganDeck.Domain;

public record ValidationResult(bool IsValid, List<string> Errors);