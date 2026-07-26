using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<List<CartaDto>>> GetCartas(){
        var cartas = await _repo.GetCartas();
        return Ok(cartas.Select(c => ToDto(c)).ToList());
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

