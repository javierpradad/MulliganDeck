using Microsoft.AspNetCore.Mvc;
using MulliganDeck.Infrastructure;
using MulliganDeck.Domain;

[ApiController]
[Route("api/cartas")]
public class CartasController : ControllerBase
{   
    private readonly CartaRepository _repo;

    public CartasController(CartaRepository repo)
    {
        _repo = repo;
    }

    //GET
    [HttpGet]
    public async Task<ActionResult<ResultadoPaginado<CartaDto>>> GetCartas(string? color, string? nombre, int page = 1, int pageSize = 20){
        // Validación de parámetros
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 100) pageSize = 100;

        var resultado = await _repo.GetCartas(color, nombre, page, pageSize);
        var dtos = resultado.Items.Select(c => ToDto(c)).ToList();
        return Ok(new ResultadoPaginado<CartaDto>(dtos, resultado.Total, resultado.Page, resultado.PageSize));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartaDto>> GetCartaPorId(int id){
        var carta = await _repo.GetCartaPorId(id);
        if (carta == null){
            return NotFound();
        }
        return Ok(ToDto(carta));
    }

    //POST
    [HttpPost]
    public async Task<ActionResult<CartaDto>> PostCarta(CrearCartaDto dto){
        var carta = ToEntity(dto);
        var creada = await _repo.AddCarta(carta);
        var salida = ToDto(creada);
        return CreatedAtAction(nameof(GetCartaPorId), new { id = salida.Id }, salida);
    }

    //PUT
    [HttpPut("{id}")]
    public async Task<ActionResult<CartaDto>> PutCarta(int id, CrearCartaDto dto){
        var carta = ToEntity(dto, id);
        var actualizada = await _repo.UpdateCarta(id, carta);
        if (actualizada == null){
            return NotFound();
        }
        return Ok(ToDto(actualizada));
    }

    //DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCarta(int id){
        if (!await _repo.DeleteCarta(id)){
            return NotFound();
        }
        return NoContent();
    }

    //Mapeo entre DTO y entidad
    private Carta ToEntity(CrearCartaDto dto)
    {
        return new Carta(0, dto.Nombre, dto.CosteMana, dto.Color, dto.Ataque, dto.Vida);
    }
    private Carta ToEntity(CrearCartaDto dto, int id)
    {
        return new Carta(id, dto.Nombre, dto.CosteMana, dto.Color, dto.Ataque, dto.Vida);
    }

    private CartaDto ToDto(Carta carta){
        return new CartaDto(carta.Id, carta.Nombre, carta.CosteMana, carta.Color, carta.Ataque, carta.Vida);
    }



}

