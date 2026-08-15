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
}