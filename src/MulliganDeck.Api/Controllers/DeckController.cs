using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MulliganDeck.Api.Dtos;
using MulliganDeck.Domain;
using MulliganDeck.Infrastructure;
using System.Security.Claims;

namespace MulliganDeck.Api.Controllers;

[ApiController]
[Route("api/decks")]
[Authorize]
public class DecksController : ControllerBase
{
    private readonly MulliganDeckContext _context;

    public DecksController(MulliganDeckContext context)
    {
        _context = context;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDeckDto dto)
    {
        var deck = new Deck
        {
            Name = dto.Name,
            Format = dto.Format,
            UserId = GetUserId()
        };

        _context.Decks.Add(deck);
        await _context.SaveChangesAsync();

        return Ok(new { deck.Id, deck.Name, deck.Format });
    }

    [HttpGet]
    public async Task<IActionResult> GetMyDecks()
    {
        var userId = GetUserId();

        var decks = await _context.Decks
            .Where(d => d.UserId == userId)
            .Select(d => new { d.Id, d.Name, d.Format })
            .ToListAsync();

        return Ok(decks);
    }

    [HttpPost("{deckId}/cards")]
    public async Task<IActionResult> AddCard(int deckId, AddCardToDeckDto dto)
    {
        var userId = GetUserId();

        var deck = await _context.Decks
            .Include(d => d.Cards)
            .FirstOrDefaultAsync(d => d.Id == deckId && d.UserId == userId);

        if (deck == null)
            return NotFound(new { message = "Mazo no encontrado." });

        var cardExists = await _context.Cards.AnyAsync(c => c.OracleId == dto.CardId);
        if (!cardExists)
            return NotFound(new { message = "Carta no encontrada." });

        var existing = deck.Cards.FirstOrDefault(dc => dc.CardId == dto.CardId);
        if (existing != null)
        {
            existing.Quantity += dto.Quantity;
        }
        else
        {
            deck.Cards.Add(new DeckCard
            {
                CardId = dto.CardId,
                Quantity = dto.Quantity
            });
        }

        await _context.SaveChangesAsync();

        return Ok(new { message = "Carta añadida." });
    }

    [HttpGet("{deckId}")]
    public async Task<IActionResult> GetDeck(int deckId)
    {
        var userId = GetUserId();

        var deck = await _context.Decks
            .Include(d => d.Cards)
                .ThenInclude(dc => dc.Card)
            .FirstOrDefaultAsync(d => d.Id == deckId && d.UserId == userId);

        if (deck == null)
            return NotFound(new { message = "Mazo no encontrado." });

        return Ok(new
        {
            deck.Id,
            deck.Name,
            deck.Format,
            Cards = deck.Cards.Select(dc => new
            {
                dc.CardId,
                CardName = dc.Card.Name,
                dc.Quantity
            })
        });
    }

    [HttpDelete("{deckId}/cards/{cardId}")]
    public async Task<IActionResult> RemoveCard(int deckId, Guid cardId)
    {
        var userId = GetUserId();

        var deck = await _context.Decks
            .Include(d => d.Cards)
            .FirstOrDefaultAsync(d => d.Id == deckId && d.UserId == userId);

        if (deck == null)
            return NotFound(new { message = "Mazo no encontrado." });

        var card = deck.Cards.FirstOrDefault(dc => dc.CardId == cardId);
        if (card == null)
            return NotFound(new { message = "La carta no está en el mazo." });

        deck.Cards.Remove(card);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{deckId}")]
    public async Task<IActionResult> DeleteDeck(int deckId)
    {
        var userId = GetUserId();

        var deck = await _context.Decks
            .FirstOrDefaultAsync(d => d.Id == deckId && d.UserId == userId);

        if (deck == null)
            return NotFound(new { message = "Mazo no encontrado." });

        _context.Decks.Remove(deck);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{deckId}/validate")]
    public async Task<IActionResult> ValidateDeck(int deckId)
    {
        var userId = GetUserId();

        var deck = await _context.Decks
            .Include(d => d.Cards)
                .ThenInclude(dc => dc.Card)
            .Include(d => d.Commander)
            .FirstOrDefaultAsync(d => d.Id == deckId && d.UserId == userId);

        if (deck == null)
            return NotFound(new { message = "Mazo no encontrado." });

        var validator = new DeckValidator();
        var result = validator.Validate(deck);

        return Ok(new
        {
            deckId = deck.Id,
            isValid = result.IsValid,
            errors = result.Errors
        });
    }
}