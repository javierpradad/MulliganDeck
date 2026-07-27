namespace MulliganDeck.Domain;

public record ResultadoPaginado<T>(
    List<T> Items, 
    int Total, 
    int Page, 
    int PageSize);