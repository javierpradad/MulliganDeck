using Microsoft.AspNetCore.Mvc;
using MulliganDeck.Infrastructure;
using MulliganDeck.Api.Dtos;
using MulliganDeck.Domain;

namespace MulliganDeck.Api;

[ApiController]
[Route("api/cards")]
public class CardsController : ControllerBase
{   
    private readonly CardRepository _repo;

    public CardsController(CardRepository repo)
    {
        _repo = repo;
    }

    //GET
    [HttpGet]
    public async Task<ActionResult<PagedResult<CardDto>>> GetCards(string? color, string? name, int page = 1, int pageSize = 20){
        // Validación de parámetros
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 100) pageSize = 100;

        var result = await _repo.GetCards(color, name, page, pageSize);
        var dtos = result.Items.Select(c => ToDto(c)).ToList();
        return Ok(new PagedResult<CardDto>(dtos, result.Total, result.Page, result.PageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CardDto>> GetCardById(Guid id){
        var card = await _repo.GetCardById(id);
        if (card == null){
            return NotFound();
        }
        return Ok(ToDto(card));
    }

    private CardDto ToDto(Card card){
        return new CardDto(card.OracleId, card.Name, card.OracleText, card.ManaCost, card.Cmc, 
                           card.Colors.ToString(), card.ColorIdentity.ToString(), card.TypeLine, 
                           card.Power, card.Toughness);
    }



}

